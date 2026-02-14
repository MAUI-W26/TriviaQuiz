using TriviaQuiz.Domain.Entities;
using TriviaQuiz.Domain.Requests;

namespace TriviaQuiz.Domain.Contracts;

public interface IQuizQuestionService
{
    Task<IReadOnlyList<QuizQuestion>> GetQuestionsAsync(
        TriviaRequest request,
        CancellationToken cancellationToken = default);

    IReadOnlyList<TriviaCategory> GetCategories();
}
