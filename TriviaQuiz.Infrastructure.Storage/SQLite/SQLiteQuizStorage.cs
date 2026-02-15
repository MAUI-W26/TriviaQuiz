using SQLite;
using System.Text.Json;
using TriviaQuiz.Domain.Contracts;
using TriviaQuiz.Domain.Entities;

namespace TriviaQuiz.Infrastructure.Storage.SQLite;

public sealed class SQLiteQuizStorage : IQuizStorage
{
    private readonly SQLiteAsyncConnection _connection;

    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web);

    public SQLiteQuizStorage(SQLiteConnectionFactory factory)
    {
        _connection = factory.Create();
    }

    public async Task<QuizSession?> LoadSessionAsync(
        CancellationToken cancellationToken = default)
    {
        var row = await _connection
            .Table<SQLiteQuizSession>()
            .FirstOrDefaultAsync();

        if (row == null)
            return null;

        return JsonSerializer.Deserialize<QuizSession>(
            row.SessionJson,
            JsonOptions);
    }

    public async Task SaveSessionAsync(
        QuizSession session,
        CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(
            session,
            JsonOptions);

        var row = new SQLiteQuizSession
        {
            Id = 1,
            SessionJson = json
        };

        await _connection.InsertOrReplaceAsync(row);
    }

    public async Task DeleteSessionAsync(
        CancellationToken cancellationToken = default)
    {
        await _connection.DeleteAllAsync<SQLiteQuizSession>();
    }

    public async Task<QuizStatistics?> LoadStatisticsAsync(
        CancellationToken cancellationToken = default)
    {
        var row = await _connection
            .Table<SQLiteQuizStatistics>()
            .FirstOrDefaultAsync();

        if (row == null)
            return null;

        return new QuizStatistics
        {
            GamesPlayed = row.GamesPlayed,
            BestScore = row.BestScore,
            TotalCorrectAnswers = row.TotalCorrectAnswers,
            TotalQuestionsAnswered = row.TotalQuestionsAnswered
        };
    }

    public async Task SaveStatisticsAsync(
        QuizStatistics statistics,
        CancellationToken cancellationToken = default)
    {
        var row = new SQLiteQuizStatistics
        {
            Id = 1,
            GamesPlayed = statistics.GamesPlayed,
            BestScore = statistics.BestScore,
            TotalCorrectAnswers = statistics.TotalCorrectAnswers,
            TotalQuestionsAnswered = statistics.TotalQuestionsAnswered
        };

        await _connection.InsertOrReplaceAsync(row);
    }
}
