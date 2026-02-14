using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaQuiz.Domain.Contracts;
using TriviaQuiz.Domain.Entities;

namespace TriviaQuiz.Infrastructure.Storage.Services
{
    public class QuizStorageFacade : IQuizStorage
    {
        public Task ClearSessionAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteSessionAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<QuizSession?> LoadSessionAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<QuizStatistics> LoadStatisticsAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task SaveSessionAsync(QuizSession session, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task SaveStatisticsAsync(QuizStatistics statistics, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
