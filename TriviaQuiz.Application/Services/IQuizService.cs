using TriviaQuiz.Domain.Entities;
using TriviaQuiz.Domain.Enums;

namespace TriviaQuiz.Application.Services;

// TODO: Considder moving to Domain.Contracts and verify projects dependencies
// (app depends on application and domain, maybe app could just depend on domain.contracts not application ...)
public interface IQuizService 
{
    IReadOnlyList<TriviaCategory> GetCategories();

    Task<bool> HasActiveSessionAsync(
        CancellationToken cancellationToken = default);

    Task<QuizSession?> ResumeSessionAsync(
        CancellationToken cancellationToken = default);

    Task<QuizSession> StartNewSessionAsync(
        int questionCount,
        Difficulty difficulty,
        string? categoryKey,
        CancellationToken cancellationToken = default);

    QuizQuestion GetCurrentQuestion();

    Task<bool> SelectAnswerAsync(
        int selectedIndex,
        CancellationToken cancellationToken = default);

    bool CanAdvance();

    Task AdvanceAsync(
        CancellationToken cancellationToken = default);

    bool IsCompleted { get; }

    QuizSession GetSession();

    Task AbandonSessionAsync(
        CancellationToken cancellationToken = default);
}
