using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using TriviaQuiz.Domain.Contracts;
using TriviaQuiz.Domain.Entities;
using TriviaQuiz.Domain.Enums;
using TriviaQuiz.Domain.Requests;
using TriviaQuiz.Infrastructure.Trivia.Services;

namespace TriviaQuiz.Application.Services;

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

    public IReadOnlyList<TriviaCategory> GetCategories()
    {
        return _questionService.GetCategories();
    }

    public async Task<bool> HasActiveSessionAsync(
        CancellationToken cancellationToken = default)
    {
        if (_session != null && !_session.IsCompleted)
            return true;

        var stored = await _storage.LoadSessionAsync(cancellationToken);

        return stored != null && !stored.IsCompleted;
    }

    public async Task<QuizSession?> ResumeSessionAsync(
        CancellationToken cancellationToken = default)
    {
        if (_session != null && !_session.IsCompleted)
            return _session;

        _session = await _storage.LoadSessionAsync(cancellationToken);

        return _session;
    }

    public async Task<QuizSession> StartNewSessionAsync(
        int questionCount,
        Difficulty difficulty,
        string? categoryKey,
        CancellationToken cancellationToken = default)
    {
        if (questionCount <= 0)
            throw new ArgumentException("Question count must be > 0.");

        _logger.LogInformation(
            "Starting new quiz session Count={Count} Difficulty={Difficulty} Category={Category}",
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
            SelectedAnswers = Enumerable.Repeat<int?>(
                null,
                questions.Count).ToList()
        };

        await _storage.SaveSessionAsync(session, cancellationToken);

        _session = session;

        return session;
    }

    public QuizQuestion GetCurrentQuestion()
    {
        EnsureSession();

        if (_session!.IsCompleted)
            throw new InvalidOperationException("Session is completed.");

        return _session.Questions[_session.CurrentQuestionIndex];
    }

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

        var selectedAnswers = _session.SelectedAnswers.ToList();
        selectedAnswers[questionIndex] = selectedIndex;

        var correct = selectedIndex == question.CorrectIndex;

        var correctCount = _session.CorrectAnswers;

        if (correct)
            correctCount++;

        _session = new QuizSession
        {
            Id = _session.Id,
            CreatedAtUtc = _session.CreatedAtUtc,
            Questions = _session.Questions,
            CurrentQuestionIndex = _session.CurrentQuestionIndex,
            CorrectAnswers = correctCount,
            SelectedAnswers = selectedAnswers,
            IsCompleted = _session.IsCompleted
        };

        await _storage.SaveSessionAsync(_session, cancellationToken);

        return correct;
    }

    public bool CanAdvance()
    {
        EnsureSession();

        return !_session!.IsCompleted;
    }

    public async Task AdvanceAsync(
        CancellationToken cancellationToken = default)
    {
        EnsureSession();

        if (_session!.IsCompleted)
            throw new InvalidOperationException("Session already completed.");

        var nextIndex = _session.CurrentQuestionIndex + 1;

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
    }

    public bool IsCompleted
    {
        get
        {
            EnsureSession();
            return _session!.IsCompleted;
        }
    }

    public QuizSession GetSession()
    {
        EnsureSession();
        return _session!;
    }

    public async Task AbandonSessionAsync(
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Abandoning session");

        _session = null;

        await _storage.DeleteSessionAsync(cancellationToken);
    }

    private void EnsureSession()
    {
        if (_session == null)
            throw new InvalidOperationException("No active session.");
    }


}
