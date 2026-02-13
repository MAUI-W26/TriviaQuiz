using TriviaQuiz.Domain.Enums;

namespace TriviaQuiz.Domain.Requests;

public sealed class TriviaRequest
{
    public int QuestionCount { get; init; }

    public Difficulty Difficulty { get; init; } = Difficulty.Any;

    public bool IncludeBoolean { get; init; } = true;

    public bool IncludeTextChoice { get; init; } = true;

    public bool IncludeImageChoice { get; init; } = true;
}
