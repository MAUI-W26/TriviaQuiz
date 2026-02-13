namespace TriviaQuiz.Domain.Entities;

public sealed class QuizStatistics
{
    public int GamesPlayed { get; init; }

    public int BestScore { get; init; }

    public int TotalCorrectAnswers { get; init; }

    public int TotalQuestionsAnswered { get; init; }

    public double AverageScore =>
        TotalQuestionsAnswered == 0
            ? 0
            : (double)TotalCorrectAnswers / TotalQuestionsAnswered;
}
