using TriviaQuiz.Domain.Enums;

namespace TriviaQuiz.Domain.Entities;

public sealed class QuizQuestion
{
    public required QuestionType Type { get; init; }

    public required string QuestionText { get; init; }

    public required IReadOnlyList<AnswerOption> Options { get; init; }

    public required int CorrectIndex { get; init; }

    public string? ImageUrl { get; init; }
}
