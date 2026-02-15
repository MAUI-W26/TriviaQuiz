using TriviaQuiz.Domain.Entities;

namespace TriviaQuiz.Domain.Contracts;

public interface IQuizStorage
{
    Task<QuizSession?> LoadSessionAsync(
        CancellationToken cancellationToken = default);

    Task SaveSessionAsync(
        QuizSession session,
        CancellationToken cancellationToken = default);

    Task DeleteSessionAsync(
        CancellationToken cancellationToken = default);

    Task<QuizStatistics?> LoadStatisticsAsync(
        CancellationToken cancellationToken = default);

    Task SaveStatisticsAsync(
        QuizStatistics statistics,
        CancellationToken cancellationToken = default);
}
