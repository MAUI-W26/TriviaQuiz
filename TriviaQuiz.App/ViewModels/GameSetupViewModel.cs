using System.Collections.ObjectModel;
using System.Windows.Input;
using TriviaQuiz.Application.Services;
using TriviaQuiz.Domain.Entities;
using TriviaQuiz.Domain.Enums;

namespace TriviaQuiz.App.ViewModels;

public sealed class GameSetupViewModel
{
    private readonly IQuizService _quizService;

    public ObservableCollection<int> QuestionCountOptions { get; }

    public ObservableCollection<Difficulty> DifficultyOptions { get; }

    public ObservableCollection<TriviaCategory> CategoryOptions { get; }

    public int SelectedQuestionCount { get; set; }

    public Difficulty SelectedDifficulty { get; set; }

    public TriviaCategory? SelectedCategory { get; set; }

    public ICommand StartQuizCommand { get; }

    public GameSetupViewModel(IQuizService quizService)
    {
        _quizService = quizService;

        QuestionCountOptions =
            new ObservableCollection<int>
            {
                5,
                10,
                15,
                20
            };

        DifficultyOptions =
            new ObservableCollection<Difficulty>(
                Enum.GetValues<Difficulty>());

        CategoryOptions =
            new ObservableCollection<TriviaCategory>(
                _quizService.GetCategories());

        SelectedQuestionCount = 10;

        SelectedDifficulty = Difficulty.Medium;

        StartQuizCommand =
            new Command(async () => await StartQuizAsync());
    }

    private async Task StartQuizAsync()
    {
        var categoryKey =
            SelectedCategory?.Key;

        await _quizService.StartNewSessionAsync(
            SelectedQuestionCount,
            SelectedDifficulty,
            categoryKey);

        await Shell.Current.GoToAsync(
            nameof(Views.QuizPage));
    }
}
