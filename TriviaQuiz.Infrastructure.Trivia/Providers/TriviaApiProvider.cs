using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using TriviaQuiz.Domain.Enums;
using TriviaQuiz.Domain.Requests;
using TriviaQuiz.Infrastructure.Trivia.Adapters;
using TriviaQuiz.Infrastructure.Trivia.DTOs;

namespace TriviaQuiz.Infrastructure.Trivia.Providers;

public sealed class TriviaApiProvider : TriviaProviderBase
{
    public TriviaApiProvider(HttpClient http, ILogger? logger = null)
        : base(http, logger)
    {
    }

    public override IReadOnlySet<QuestionType> SupportedTypes { get; }
        = new HashSet<QuestionType>
        {
            QuestionType.Choice
        };

    protected override async Task<object> FetchRawAsync(
        TriviaRequest request,
        CancellationToken cancellationToken)
    {
        var url = BuildUrl(request);

        Logger.LogInformation("Trivia API request: {Url}", url);

        var dto = await Http.GetFromJsonAsync<List<TriviaApiQuestionDto>>(
            url,
            cancellationToken);

        if (dto == null)
            throw new HttpRequestException("Trivia API returned an empty response.");

        return dto;
    }

    protected override List<TriviaQuiz.Domain.Entities.QuizQuestion> MapToDomain(object raw)
    {
        var items = raw as List<TriviaApiQuestionDto>
            ?? throw new InvalidOperationException("Trivia API raw payload shape was unexpected.");

        return items.Select(TriviaApiAdapter.Map).ToList();
    }

    private static string BuildUrl(TriviaRequest request)
    {
        var qs = new List<string>
        {
            $"limit={request.QuestionCount}",
            "types=text_choice"
        };

        var difficulty = request.Difficulty switch
        {
            Difficulty.Easy => "easy",
            Difficulty.Medium => "medium",
            Difficulty.Hard => "hard",
            _ => null
        };

        if (difficulty != null)
            qs.Add($"difficulties={difficulty}");

        if (!string.IsNullOrWhiteSpace(request.CategoryKey))
            qs.Add($"categories={request.CategoryKey}");

        return "https://the-trivia-api.com/v2/questions?" + string.Join("&", qs);
    }
}
