using SQLite;

namespace TriviaQuiz.Infrastructure.Storage.SQLite;

[Table("quiz_session")]
public sealed class SQLiteQuizSession
{
    [PrimaryKey]
    public int Id { get; set; } = 1;

    public string SessionJson { get; set; } = string.Empty;
}

[Table("quiz_statistics")]
public sealed class SQLiteQuizStatistics
{
    [PrimaryKey]
    public int Id { get; set; } = 1;

    public int GamesPlayed { get; set; }

    public int BestScore { get; set; }

    public int TotalCorrectAnswers { get; set; }

    public int TotalQuestionsAnswered { get; set; }
}
