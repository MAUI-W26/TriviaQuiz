using TriviaQuiz.Domain.Entities;

namespace TriviaQuiz.Infrastructure.Trivia.Utilities;

/// <summary>
/// Validates QuizQuestion domain integrity.
/// Ensures provider mapping produces structurally valid questions.
/// </summary>
public static class QuestionValidator
{
    public static void ThrowIfInvalid(QuizQuestion q)
    {
        if (q == null)
            throw new InvalidOperationException("QuizQuestion cannot be null.");

        if (string.IsNullOrWhiteSpace(q.QuestionText))
            throw new InvalidOperationException(
                "QuestionText cannot be empty.");

        if (q.Options == null || q.Options.Count < 2)
            throw new InvalidOperationException(
                "A question must have at least two options.");

        if (q.Options.Any(o => string.IsNullOrWhiteSpace(o)))
            throw new InvalidOperationException(
                "Options cannot contain empty values.");

        if (q.CorrectIndex < 0 ||
            q.CorrectIndex >= q.Options.Count)
            throw new InvalidOperationException(
                "CorrectIndex is out of bounds.");
    }
}
