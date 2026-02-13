namespace TriviaQuiz.Domain.Entities;

public sealed class QuizSession
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public DateTime CreatedAtUtc { get; init; } = DateTime.UtcNow;

    public IReadOnlyList<QuizQuestion> Questions { get; init; } = [];

    public int CurrentQuestionIndex { get; init; }

    public int CorrectAnswers { get; init; }

    public bool IsCompleted { get; init; }

    public IReadOnlyList<int?> SelectedAnswers { get; init; } = [];
}
