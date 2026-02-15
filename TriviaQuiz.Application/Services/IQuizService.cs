using TriviaQuiz.Domain.Entities;
using TriviaQuiz.Domain.Enums;

namespace TriviaQuiz.Application.Services;

/// <summary>
/// Application service facade that exposes all quiz-related operations.
/// 
/// This service coordinates:
/// - Question retrieval from trivia providers
/// - Session lifecycle management
/// - Answer selection and progression
/// - Persistent storage of session and statistics
/// 
/// The UI layer must only interact with this interface and must never access
/// storage or trivia providers directly.
/// </summary>
public interface IQuizService
{
    /// <summary>
    /// Gets the list of available trivia categories supported by the current provider.
    /// </summary>
    IReadOnlyList<TriviaCategory> GetCategories();

    /// <summary>
    /// Determines whether a non-completed session exists either in memory or persistent storage.
    /// </summary>
    Task<bool> HasActiveSessionAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Attempts to resume a previously persisted session.
    /// Returns null if no active session exists.
    /// </summary>
    Task<QuizSession?> ResumeSessionAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Starts a new quiz session using the specified parameters.
    /// Any existing session will be replaced.
    /// </summary>
    Task<QuizSession> StartNewSessionAsync(
        int questionCount,
        Difficulty difficulty,
        string? categoryKey,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current question for the active session.
    /// Throws if no session exists or if the session is completed.
    /// </summary>
    QuizQuestion GetCurrentQuestion();

    /// <summary>
    /// Selects an answer for the current question and persists the updated session.
    /// Returns true if the selected answer is correct.
    /// </summary>
    Task<bool> SelectAnswerAsync(
        int selectedIndex,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Indicates whether the session can advance to the next question.
    /// </summary>
    bool CanAdvance();

    /// <summary>
    /// Advances the session to the next question or completes the session if at the end.
    /// When completing, statistics are automatically updated and persisted.
    /// </summary>
    Task AdvanceAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Indicates whether the active session has been completed.
    /// </summary>
    bool IsCompleted { get; }

    /// <summary>
    /// Gets the current active session.
    /// </summary>
    QuizSession GetSession();

    /// <summary>
    /// Abandons the current session and removes it from persistent storage.
    /// Statistics are not affected.
    /// </summary>
    Task AbandonSessionAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Loads persistent quiz statistics.
    /// Returns an empty statistics object if none exist.
    /// </summary>
    Task<QuizStatistics> GetStatisticsAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Resets persistent statistics to their initial state.
    /// </summary>
    Task ResetStatisticsAsync(
        CancellationToken cancellationToken = default);
}
