namespace TriviaQuiz.Domain.Entities;

public sealed class QuizSession
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public DateTime CreatedAtUtc { get; init; } = DateTime.UtcNow;

    public required IReadOnlyList<QuizQuestion> Questions { get; init; }

    public required IReadOnlyList<int?> SelectedAnswers { get; init; }

    public required int CurrentQuestionIndex { get; init; }

    public required int CorrectAnswers { get; init; }

    public required bool IsCompleted { get; init; }
}
