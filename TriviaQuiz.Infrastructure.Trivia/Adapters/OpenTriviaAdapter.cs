using System.Net;
using TriviaQuiz.Domain.Entities;
using TriviaQuiz.Domain.Enums;
using TriviaQuiz.Infrastructure.Trivia.DTOs;

namespace TriviaQuiz.Infrastructure.Trivia.Adapters;

internal static class OpenTriviaAdapter
{
    public static QuizQuestion Map(OpenTriviaDbQuestionDto dto)
    {
        var type = dto.Type switch
        {
            "boolean" => QuestionType.Boolean,
            "multiple" => QuestionType.Choice,
            _ => throw new NotSupportedException(
                $"Unsupported OpenTDB question type: '{dto.Type}'.")
        };

        var correct = WebUtility.HtmlDecode(dto.CorrectAnswer);

        var incorrect = dto.IncorrectAnswers
            .Select(WebUtility.HtmlDecode)
            .ToList();

        var options = incorrect
            .Append(correct)
            .OrderBy(_ => Guid.NewGuid())
            .ToList();

        var correctIndex = options.IndexOf(correct);

        return new QuizQuestion
        {
            Type = type,
            QuestionText = WebUtility.HtmlDecode(dto.Question),
            Options = options,
            CorrectIndex = correctIndex
        };
    }
}
