using TriviaQuiz.Domain.Entities;

namespace TriviaQuiz.Infrastructure.Trivia.Utilities;

public static class QuestionValidator
{
    public static void ThrowIfInvalid(QuizQuestion q)
    {
        if (string.IsNullOrWhiteSpace(q.QuestionText))
            throw new InvalidOperationException("QuestionText cannot be empty.");

        if (q.Options == null || q.Options.Count < 2)
            throw new InvalidOperationException("A question must have at least 2 options.");

        if (q.CorrectIndex < 0 || q.CorrectIndex >= q.Options.Count)
            throw new InvalidOperationException("CorrectIndex is out of bounds.");

        if (q.Options.Any(o => string.IsNullOrWhiteSpace(o.Text)))
            throw new InvalidOperationException("AnswerOption.Text cannot be empty.");
    }
}
