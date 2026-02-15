using TriviaQuiz.App.Views.QuestionViews;
using TriviaQuiz.Domain.Entities;
using TriviaQuiz.Domain.Enums;

namespace TriviaQuiz.App.Factories;

public static class QuestionViewFactory
{
    public static ContentView Create(
        QuizQuestion question,
        Action<int> answerSelected)
    {
        return question.Type switch
        {
            QuestionType.Boolean =>
                new BooleanQuestionView(
                    question,
                    answerSelected),

            QuestionType.Choice =>
                new ChoiceQuestionView(
                    question,
                    answerSelected),

            _ => throw new NotSupportedException()
        };
    }
}
