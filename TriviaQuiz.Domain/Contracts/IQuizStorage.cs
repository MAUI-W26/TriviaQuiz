using TriviaQuiz.Domain.Entities;

namespace TriviaQuiz.Domain.Contracts;

public interface IQuizStorage
{
    Task SaveSessionAsync(
        QuizSession session,
        CancellationToken cancellationToken = default); // cancellationToken -> propagate cancellation from UI to storage layer, allowing for graceful cancellation of async operations

    Task<QuizSession?> LoadSessionAsync(
        CancellationToken cancellationToken = default);

    Task SaveStatisticsAsync(
        QuizStatistics statistics,
        CancellationToken cancellationToken = default);

    Task<QuizStatistics> LoadStatisticsAsync(
        CancellationToken cancellationToken = default);

    Task ClearSessionAsync(
        CancellationToken cancellationToken = default);
    Task DeleteSessionAsync(CancellationToken cancellationToken);
}
