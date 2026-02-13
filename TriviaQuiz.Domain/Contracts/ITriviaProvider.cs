using TriviaQuiz.Domain.Entities;
using TriviaQuiz.Domain.Requests;

namespace TriviaQuiz.Domain.Contracts;

public interface ITriviaProvider
{
    Task<IReadOnlyList<QuizQuestion>> GetQuestionsAsync(
        TriviaRequest request,
        CancellationToken cancellationToken = default);
}
