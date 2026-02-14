using TriviaQuiz.Domain.Entities;
using TriviaQuiz.Domain.Enums;
using TriviaQuiz.Domain.Requests;

namespace TriviaQuiz.Infrastructure.Trivia.Providers
{
    internal interface ITriviaProvider
    {
        IReadOnlySet<QuestionType> SupportedTypes { get; }
        Task<IReadOnlyList<QuizQuestion>> GetQuestionsAsync(
            TriviaRequest request,
            CancellationToken cancellationToken = default);
    }
}
