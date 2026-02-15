using TriviaQuiz.Domain.Contracts;
using TriviaQuiz.Domain.Entities;

namespace TriviaQuiz.Infrastructure.Storage.Services;

public sealed class QuizStorageFacade : IQuizStorage
{
    private readonly IQuizStorage _primary;
    private readonly IQuizStorage? _fallback;

    public QuizStorageFacade(
        IQuizStorage primary,
        IQuizStorage? fallback = null)
    {
        _primary = primary;
        _fallback = fallback;
    }

    public async Task<QuizSession?> LoadSessionAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _primary.LoadSessionAsync(cancellationToken);
        }
        catch
        {
            if (_fallback != null)
                return await _fallback.LoadSessionAsync(cancellationToken);

            throw;
        }
    }

    public Task SaveSessionAsync(
        QuizSession session,
        CancellationToken cancellationToken = default)
        => _primary.SaveSessionAsync(session, cancellationToken);

    public Task DeleteSessionAsync(
        CancellationToken cancellationToken = default)
        => _primary.DeleteSessionAsync(cancellationToken);

    public async Task<QuizStatistics?> LoadStatisticsAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _primary.LoadStatisticsAsync(cancellationToken);
        }
        catch
        {
            if (_fallback != null)
                return await _fallback.LoadStatisticsAsync(cancellationToken);

            throw;
        }
    }

    public Task SaveStatisticsAsync(
        QuizStatistics statistics,
        CancellationToken cancellationToken = default)
        => _primary.SaveStatisticsAsync(statistics, cancellationToken);
}
