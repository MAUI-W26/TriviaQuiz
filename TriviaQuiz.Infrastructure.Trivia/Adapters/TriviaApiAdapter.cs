using TriviaQuiz.Domain.Entities;
using TriviaQuiz.Domain.Enums;
using TriviaQuiz.Infrastructure.Trivia.DTOs;

namespace TriviaQuiz.Infrastructure.Trivia.Adapters;

internal static class TriviaApiAdapter
{
    public static QuizQuestion Map(TriviaApiQuestionDto dto)
    {
        if (!string.Equals(dto.Type, "text_choice", StringComparison.OrdinalIgnoreCase))
            throw new NotSupportedException(
                $"Unsupported Trivia API question type: '{dto.Type}'.");

        var correct = dto.CorrectAnswer;

        var options = dto.IncorrectAnswers
            .Append(correct)
            .OrderBy(_ => Guid.NewGuid())
            .ToList();

        var correctIndex = options.IndexOf(correct);

        return new QuizQuestion
        {
            Type = QuestionType.Choice,
            QuestionText = dto.Question.Text,
            Options = options,
            CorrectIndex = correctIndex
        };
    }
}
