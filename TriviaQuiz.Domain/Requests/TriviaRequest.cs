using TriviaQuiz.Domain.Enums;

namespace TriviaQuiz.Domain.Requests;

public sealed class TriviaRequest
{
    public required int QuestionCount { get; init; }

    public Difficulty Difficulty { get; init; } = Difficulty.Any;

    // null = any category
    public string? CategoryKey { get; init; }

    public bool IncludeBoolean { get; init; } = true;

    public bool IncludeChoice { get; init; } = true;

}
