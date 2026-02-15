using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using TriviaQuiz.Domain.Contracts;
using TriviaQuiz.Domain.Entities;
using TriviaQuiz.Domain.Enums;
using TriviaQuiz.Domain.Requests;
using TriviaQuiz.Infrastructure.Trivia.Services;

namespace TriviaQuiz.Application.Services;

/// <summary>
/// Default implementation of the quiz application service.
/// 
/// This service acts as the orchestration layer between:
/// - Trivia question providers (external infrastructure)
/// - Persistent storage (session and statistics)
/// - Application/UI layer
/// 
/// Responsibilities:
/// - Manage quiz session lifecycle
/// - Coordinate persistence through storage facade
/// - Enforce game rules and progression
/// - Maintain and update cumulative statistics
/// 
/// This class contains no UI or platform-specific logic.
/// </summary>
public sealed class QuizService : IQuizService
{
    private readonly IQuizQuestionService _questionService;
    private readonly IQuizStorage _storage;
    private readonly ILogger<QuizService> _logger;

    private QuizSession? _session;

    public QuizService(
        IQuizQuestionService questionService,
        IQuizStorage storage,
        ILogger<QuizService>? logger = null)
    {
        _questionService = questionService
            ?? throw new ArgumentNullException(nameof(questionService));

        _storage = storage
            ?? throw new ArgumentNullException(nameof(storage));

        _logger = logger ?? NullLogger<QuizService>.Instance;
    }

    /// <inheritdoc/>
    public IReadOnlyList<TriviaCategory> GetCategories()
    {
        return _questionService.GetCategories();
    }

    /// <inheritdoc/>
    public async Task<bool> HasActiveSessionAsync(
        CancellationToken cancellationToken = default)
    {
        if (_session != null && !_session.IsCompleted)
            return true;

        var stored = await _storage.LoadSessionAsync(cancellationToken);

        return stored != null && !stored.IsCompleted;
    }

    /// <inheritdoc/>
    public async Task<QuizSession?> ResumeSessionAsync(
        CancellationToken cancellationToken = default)
    {
        if (_session != null && !_session.IsCompleted)
            return _session;

        _session = await _storage.LoadSessionAsync(cancellationToken);

        return _session;
    }

    /// <inheritdoc/>
    public async Task<QuizSession> StartNewSessionAsync(
        int questionCount,
        Difficulty difficulty,
        string? categoryKey,
        CancellationToken cancellationToken = default)
    {
        if (questionCount <= 0)
            throw new ArgumentException("Question count must be greater than zero.");

        _logger.LogInformation(
            "Starting quiz session: Count={Count}, Difficulty={Difficulty}, Category={Category}",
            questionCount,
            difficulty,
            categoryKey);

        var request = new TriviaRequest
        {
            QuestionCount = questionCount,
            Difficulty = difficulty,
            CategoryKey = categoryKey,
            IncludeBoolean = true,
            IncludeChoice = true
        };

        var questions = await _questionService.GetQuestionsAsync(
            request,
            cancellationToken);

        var session = new QuizSession
        {
            Questions = questions,
            CurrentQuestionIndex = 0,
            CorrectAnswers = 0,
            IsCompleted = false,
            SelectedAnswers = Enumerable
                .Repeat<int?>(null, questions.Count)
                .ToList()
        };

        await _storage.SaveSessionAsync(session, cancellationToken);

        _session = session;

        return session;
    }

    /// <inheritdoc/>
    public QuizQuestion GetCurrentQuestion()
    {
        EnsureSession();

        if (_session!.IsCompleted)
            throw new InvalidOperationException("Session is completed.");

        return _session.Questions[_session.CurrentQuestionIndex];
    }

    /// <inheritdoc/>
    public async Task<bool> SelectAnswerAsync(
        int selectedIndex,
        CancellationToken cancellationToken = default)
    {
        EnsureSession();

        var questionIndex = _session!.CurrentQuestionIndex;
        var question = _session.Questions[questionIndex];

        if (selectedIndex < 0 || selectedIndex >= question.Options.Count)
            throw new ArgumentOutOfRangeException(nameof(selectedIndex));

        if (_session.SelectedAnswers[questionIndex] != null)
            throw new InvalidOperationException("Answer already selected.");

        var updatedAnswers = _session.SelectedAnswers.ToList();
        updatedAnswers[questionIndex] = selectedIndex;

        var isCorrect = selectedIndex == question.CorrectIndex;

        var updatedCorrectCount = _session.CorrectAnswers;
        if (isCorrect)
            updatedCorrectCount++;

        _session = new QuizSession
        {
            Id = _session.Id,
            CreatedAtUtc = _session.CreatedAtUtc,
            Questions = _session.Questions,
            CurrentQuestionIndex = questionIndex,
            CorrectAnswers = updatedCorrectCount,
            SelectedAnswers = updatedAnswers,
            IsCompleted = false
        };

        await _storage.SaveSessionAsync(_session, cancellationToken);

        return isCorrect;
    }

    /// <inheritdoc/>
    public bool CanAdvance()
    {
        EnsureSession();
        return !_session!.IsCompleted;
    }

    /// <inheritdoc/>
    public async Task AdvanceAsync(
        CancellationToken cancellationToken = default)
    {
        EnsureSession();

        var nextIndex = _session!.CurrentQuestionIndex + 1;
        var completed = nextIndex >= _session.Questions.Count;

        _session = new QuizSession
        {
            Id = _session.Id,
            CreatedAtUtc = _session.CreatedAtUtc,
            Questions = _session.Questions,
            CurrentQuestionIndex = completed
                ? _session.CurrentQuestionIndex
                : nextIndex,
            CorrectAnswers = _session.CorrectAnswers,
            SelectedAnswers = _session.SelectedAnswers,
            IsCompleted = completed
        };

        await _storage.SaveSessionAsync(_session, cancellationToken);

        if (completed)
            await UpdateStatisticsAsync(_session, cancellationToken);
    }

    /// <inheritdoc/>
    public bool IsCompleted
    {
        get
        {
            EnsureSession();
            return _session!.IsCompleted;
        }
    }

    /// <inheritdoc/>
    public QuizSession GetSession()
    {
        EnsureSession();
        return _session!;
    }

    /// <inheritdoc/>
    public async Task AbandonSessionAsync(
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Abandoning quiz session.");

        _session = null;

        await _storage.DeleteSessionAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<QuizStatistics> GetStatisticsAsync(
        CancellationToken cancellationToken = default)
    {
        var stats = await _storage.LoadStatisticsAsync(cancellationToken);

        return stats ?? new QuizStatistics();
    }

    /// <inheritdoc/>
    public async Task ResetStatisticsAsync(
        CancellationToken cancellationToken = default)
    {
        await _storage.SaveStatisticsAsync(
            new QuizStatistics(),
            cancellationToken);
    }

    /// <summary>
    /// Updates cumulative statistics when a session is completed.
    /// </summary>
    private async Task UpdateStatisticsAsync(
        QuizSession session,
        CancellationToken cancellationToken)
    {
        var stats =
            await _storage.LoadStatisticsAsync(cancellationToken)
            ?? new QuizStatistics();

        var answeredCount =
            session.SelectedAnswers.Count(a => a != null);

        var updatedStats = new QuizStatistics
        {
            GamesPlayed = stats.GamesPlayed + 1,
            BestScore = Math.Max(stats.BestScore, session.CorrectAnswers),
            TotalCorrectAnswers =
                stats.TotalCorrectAnswers + session.CorrectAnswers,
            TotalQuestionsAnswered =
                stats.TotalQuestionsAnswered + answeredCount
        };

        await _storage.SaveStatisticsAsync(
            updatedStats,
            cancellationToken);
    }

    /// <summary>
    /// Ensures an active session exists before performing session operations.
    /// </summary>
    private void EnsureSession()
    {
        if (_session == null)
            throw new InvalidOperationException("No active session.");
    }
}
