using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TriviaQuiz.Application.Services;

namespace TriviaQuiz.App.ViewModels;

public sealed class MainMenuViewModel : INotifyPropertyChanged //required after async storage check completed, to update UI with CanResume value
{
    private readonly IQuizService _quizService;

    private bool _canResume;

    public bool CanResume
    {
        get => _canResume;
        private set
        {
            if (_canResume == value)
                return;

            _canResume = value;
            OnPropertyChanged();
        }
    }

    public ICommand ContinueCommand { get; }

    public ICommand StartNewGameCommand { get; }

    public ICommand ShowStatisticsCommand { get; }

    public ICommand ExitCommand { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public MainMenuViewModel(IQuizService quizService)
    {
        _quizService = quizService;

        ContinueCommand =
            new Command(async () =>
            {
                await _quizService.ResumeSessionAsync();

                await Shell.Current.GoToAsync(nameof(Views.QuizPage));
            });

        StartNewGameCommand =
            new Command(async () =>
            {
                await Shell.Current.GoToAsync(nameof(Views.GameSetupPage));
            });

        ShowStatisticsCommand =
            new Command(async () =>
            {
                await Shell.Current.GoToAsync(nameof(Views.StatisticsPage));
            });

        ExitCommand =
            new Command(() =>
                Microsoft.Maui.Controls.Application.Current?.Quit());

        Initialize();
    }

    private async void Initialize()
    {
        CanResume =
            await _quizService.HasActiveSessionAsync();
    }

    private void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}
