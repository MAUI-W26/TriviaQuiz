using Microsoft.Extensions.Logging;
using TriviaQuiz.Domain.Contracts;
using TriviaQuiz.Infrastructure.Trivia.Providers;

namespace TriviaQuiz.Infrastructure.Trivia.Factories;

internal sealed class TriviaProviderFactory
{
    private readonly HttpClient _http;
    private readonly ILoggerFactory? _loggerFactory;

    public TriviaProviderFactory(HttpClient http, ILoggerFactory? loggerFactory)
    {
        _http = http ?? throw new ArgumentNullException(nameof(http));
        _loggerFactory = loggerFactory;
    }

    public ITriviaProvider CreatePrimary()
    {
        var logger = _loggerFactory?.CreateLogger<OpenTriviaDbProvider>();
        return new OpenTriviaDbProvider(_http, logger);
    }

    public ITriviaProvider? CreateNextFallback(int attempt)
    {
        if (attempt != 0)
            return null;

        var logger = _loggerFactory?.CreateLogger<TriviaApiProvider>();
        return new TriviaApiProvider(_http, logger);
    }
}
