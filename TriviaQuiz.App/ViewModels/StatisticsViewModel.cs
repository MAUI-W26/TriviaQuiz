using System.Windows.Input;
using TriviaQuiz.Application.Services;
using TriviaQuiz.Domain.Entities;

namespace TriviaQuiz.App.ViewModels;

public sealed class StatisticsViewModel
{
    private readonly IQuizService _quizService;

    public string GamesPlayedText { get; private set; } = "0";

    public string BestScoreText { get; private set; } = "0";

    public string AccuracyText { get; private set; } = "0%";

    public ICommand ResetCommand { get; }

    public ICommand BackCommand { get; }

    public event Action? StatisticsChanged;

    public StatisticsViewModel(IQuizService quizService)
    {
        _quizService = quizService;

        ResetCommand =
            new Command(async () =>
            {
                await _quizService.ResetStatisticsAsync();
                await LoadAsync();
            });

        BackCommand =
            new Command(async () =>
            {
                await Shell.Current.GoToAsync("..");
            });
    }

    public async Task LoadAsync()
    {
        QuizStatistics stats;

        try
        {
            stats = await _quizService.GetStatisticsAsync();
        }
        catch
        {
            stats = new QuizStatistics();

            await _quizService.ResetStatisticsAsync();
        }

        GamesPlayedText =
            stats.GamesPlayed.ToString();

        BestScoreText =
            stats.BestScore.ToString();

        AccuracyText =
            ComputeAccuracy(stats);

        StatisticsChanged?.Invoke();
    }

    private static string ComputeAccuracy(QuizStatistics stats)
    {
        if (stats.TotalQuestionsAnswered == 0)
            return "0%";

        var accuracy =
            (double)stats.TotalCorrectAnswers /
            stats.TotalQuestionsAnswered;

        return $"{accuracy:P0}";
    }
}
