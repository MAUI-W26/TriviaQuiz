using SQLite;

namespace TriviaQuiz.Infrastructure.Storage.SQLite;

public sealed class SQLiteConnectionFactory
{
    private readonly string _dbPath;

    public SQLiteConnectionFactory(string dbPath)
    {
        _dbPath = dbPath;
    }

    public SQLiteAsyncConnection Create()
    {
        var connection = new SQLiteAsyncConnection(_dbPath);

        connection.CreateTableAsync<SQLiteQuizSession>().Wait();
        connection.CreateTableAsync<SQLiteQuizStatistics>().Wait();

        return connection;
    }
}
