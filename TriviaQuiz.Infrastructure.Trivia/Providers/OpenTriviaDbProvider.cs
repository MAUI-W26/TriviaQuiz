using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using TriviaQuiz.Domain.Enums;
using TriviaQuiz.Domain.Requests;
using TriviaQuiz.Infrastructure.Trivia.Adapters;
using TriviaQuiz.Infrastructure.Trivia.DTOs;
using TriviaQuiz.Infrastructure.Trivia.Utilities;

namespace TriviaQuiz.Infrastructure.Trivia.Providers;

public sealed class OpenTriviaDbProvider : TriviaProviderBase
{
    public OpenTriviaDbProvider(HttpClient http, ILogger? logger = null)
        : base(http, logger)
    {
    }

    public override IReadOnlySet<QuestionType> SupportedTypes { get; }
        = new HashSet<QuestionType>
        {
            QuestionType.Boolean,
            QuestionType.Choice
        };

    protected override async Task<object> FetchRawAsync( // actual http request
        TriviaRequest request,
        CancellationToken cancellationToken)
    {
        var url = BuildUrl(request);

        Logger.LogInformation("OpenTDB request: {Url}", url);

        var dto = await Http.GetFromJsonAsync<OpenTriviaDbResponseDto>(
            url,
            cancellationToken);

        if (dto == null)
            throw new HttpRequestException("OpenTDB returned an empty response.");

        if (dto.ResponseCode != 0)
            throw new InvalidOperationException($"OpenTDB response_code={dto.ResponseCode}.");

        return dto.Results;
    }

    protected override List<TriviaQuiz.Domain.Entities.QuizQuestion> MapToDomain(object raw)
    {
        var items = raw as List<OpenTriviaDbQuestionDto>
            ?? throw new InvalidOperationException("OpenTDB raw payload shape was unexpected.");

        return items.Select(OpenTriviaAdapter.Map).ToList();
    }

    private static string BuildUrl(TriviaRequest request)
    {
        var qs = new List<string>
        {
            $"amount={request.QuestionCount}"
        };

        var difficulty = request.Difficulty switch
        {
            Difficulty.Easy => "easy",
            Difficulty.Medium => "medium",
            Difficulty.Hard => "hard",
            _ => null
        };

        if (difficulty != null)
            qs.Add($"difficulty={difficulty}");

        if (!string.IsNullOrWhiteSpace(request.CategoryKey) &&
            OpenTriviaCategoryMap.TryGetId(request.CategoryKey, out var id))
        {
            qs.Add($"category={id}");
        }

        var type = ResolveType(request);
        if (type != null)
            qs.Add($"type={type}");

        return "https://opentdb.com/api.php?" + string.Join("&", qs);
    }

    private static string? ResolveType(TriviaRequest request)
    {
        if (request.IncludeBoolean && !request.IncludeChoice)
            return "boolean";

        if (!request.IncludeBoolean && request.IncludeChoice)
            return "multiple";

        return null;
    }
}
