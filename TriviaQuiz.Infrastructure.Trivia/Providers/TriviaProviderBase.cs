using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using TriviaQuiz.Domain.Contracts;
using TriviaQuiz.Domain.Entities;
using TriviaQuiz.Domain.Enums;
using TriviaQuiz.Domain.Requests;
using TriviaQuiz.Infrastructure.Trivia.Utilities;

namespace TriviaQuiz.Infrastructure.Trivia.Providers;

public abstract class TriviaProviderBase : ITriviaProvider
{
    protected readonly HttpClient Http;
    protected readonly ILogger Logger;

    protected TriviaProviderBase(HttpClient http, ILogger? logger)
    {
        Http = http;
        Logger = logger ?? NullLogger.Instance;
    }

    public abstract IReadOnlySet<QuestionType> SupportedTypes { get; }

    public async Task<IReadOnlyList<QuizQuestion>> GetQuestionsAsync(
        TriviaRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var raw = await FetchRawAsync(request, cancellationToken);
            var mapped = MapToDomain(raw);

            var filtered = mapped
                .Where(q => SupportedTypes.Contains(q.Type))
                .ToList();

            foreach (var q in filtered)
                QuestionValidator.ThrowIfInvalid(q);

            return filtered;
        }
        catch (OperationCanceledException)
        {
            Logger.LogWarning("Provider call cancelled.");
            throw;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Provider failed.");
            throw;
        }
    }

    protected abstract Task<object> FetchRawAsync(
        TriviaRequest request,
        CancellationToken cancellationToken);

    protected abstract List<QuizQuestion> MapToDomain(object raw);
}
