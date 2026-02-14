using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using TriviaQuiz.Domain.Contracts;
using TriviaQuiz.Domain.Entities;
using TriviaQuiz.Domain.Requests;
using TriviaQuiz.Infrastructure.Trivia.Factories;
using TriviaQuiz.Infrastructure.Trivia.Providers;

namespace TriviaQuiz.Infrastructure.Trivia.Services;

public sealed class QuizQuestionService : IQuizQuestionService
{
    private readonly TriviaProviderFactory _factory;
    private readonly ILogger<QuizQuestionService> _logger;

    private const int PrimaryMaxAttempts = 2;
    private const int FallbackMaxAttempts = 2;

    public QuizQuestionService(
        HttpClient httpClient,
        ILoggerFactory? loggerFactory = null,
        ILogger<QuizQuestionService>? logger = null)
    {
        if (httpClient == null)
            throw new ArgumentNullException(nameof(httpClient));

        _factory = new TriviaProviderFactory(httpClient, loggerFactory);
        _logger = logger ?? NullLogger<QuizQuestionService>.Instance;
    }

    public async Task<IReadOnlyList<QuizQuestion>> GetQuestionsAsync(
        TriviaRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (request.QuestionCount <= 0)
            throw new ArgumentException("QuestionCount must be greater than zero.", nameof(request));

        var result = new List<QuizQuestion>(request.QuestionCount);

        // PRIMARY attempts
        for (int attempt = 1; attempt <= PrimaryMaxAttempts; attempt++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var provider = _factory.CreatePrimary();

            if (await TryFetch(provider, request, result, cancellationToken, $"Primary attempt {attempt}"))
                break;

            if (result.Count >= request.QuestionCount)
                break;
        }

        // FALLBACK attempts if needed
        var remaining = request.QuestionCount - result.Count;

        if (remaining > 0)
        {
            var fallbackRequest = new TriviaRequest
            {
                QuestionCount = remaining,
                Difficulty = request.Difficulty,
                CategoryKey = request.CategoryKey,
                IncludeBoolean = false,
                IncludeChoice = true
            };

            for (int providerIndex = 0; ; providerIndex++)
            {
                var provider = _factory.CreateNextFallback(providerIndex);

                if (provider == null)
                    break;

                for (int attempt = 1; attempt <= FallbackMaxAttempts; attempt++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (await TryFetch(
                        provider,
                        fallbackRequest,
                        result,
                        cancellationToken,
                        $"Fallback[{providerIndex}] attempt {attempt}"))
                    {
                        break;
                    }

                    remaining = request.QuestionCount - result.Count;

                    if (remaining <= 0)
                        break;

                    fallbackRequest = new TriviaRequest
                    {
                        QuestionCount = remaining,
                        Difficulty = request.Difficulty,
                        CategoryKey = request.CategoryKey,
                        IncludeBoolean = false,
                        IncludeChoice = true
                    };
                }

                if (result.Count >= request.QuestionCount)
                    break;
            }
        }

        if (result.Count < request.QuestionCount)
        {
            throw new InvalidOperationException(
                $"Unable to retrieve enough questions. Requested={request.QuestionCount} Received={result.Count}");
        }

        return Shuffle(result)
            .Take(request.QuestionCount)
            .ToList();
    }

    private async Task<bool> TryFetch(
        ITriviaProvider provider,
        TriviaRequest request,
        List<QuizQuestion> result,
        CancellationToken cancellationToken,
        string label)
    {
        try
        {
            var questions = await provider.GetQuestionsAsync(request, cancellationToken);

            if (questions.Count == 0)
            {
                _logger.LogWarning("{Label} returned 0 questions", label);
                return false;
            }

            result.AddRange(questions);

            _logger.LogInformation(
                "{Label} success Count={Count} Total={Total}",
                label,
                questions.Count,
                result.Count);

            return true;
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "{Label} failed", label);
            return false;
        }
    }

    private static List<QuizQuestion> Shuffle(List<QuizQuestion> input)
    {
        return input.OrderBy(_ => Guid.NewGuid()).ToList();
    }

    public IReadOnlyList<TriviaCategory> GetCategories()
    {
        return TriviaCategoryRegistry.All;
    }

}
