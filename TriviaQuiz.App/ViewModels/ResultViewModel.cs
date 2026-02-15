using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using TriviaQuiz.Application.Services;
using TriviaQuiz.Domain.Entities;

namespace TriviaQuiz.App.ViewModels;

public sealed class ResultViewModel : INotifyPropertyChanged
{
    private readonly IQuizService _quizService;

    private QuizSession? _session;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string ScoreText =>
        _session == null
            ? ""
            : $"Score: {_session.CorrectAnswers} / {_session.Questions.Count}";

    public string PercentageText =>
        _session == null
            ? ""
            : $"Accuracy: {CalculatePercentage():F1}%";

    public ICommand ReturnCommand { get; }

    public ICommand StatisticsCommand { get; }

    public ResultViewModel(IQuizService quizService)
    {
        _quizService =
            quizService
            ?? throw new ArgumentNullException(nameof(quizService));

        ReturnCommand =
            new Command(async () =>
            {
                await _quizService.AbandonSessionAsync(); // Session cleanup

                await Shell.Current.GoToAsync(
                    $"//{nameof(Views.MainMenuPage)}"); // NOTE:  "//" clears the navigation stack
            });


        StatisticsCommand =
            new Command(async () =>
                await Shell.Current.GoToAsync(
                    $"//{nameof(Views.MainMenuPage)}"));
    }

    public Task InitializeAsync()
    {
        _session =
            _quizService.GetSession();

        NotifyStateChanged();

        return Task.CompletedTask;
    }

    private double CalculatePercentage()
    {
        if (_session == null)
            return 0;

        return
            (double)_session.CorrectAnswers /
            _session.Questions.Count * 100;
    }

    private void NotifyStateChanged()
    {
        OnPropertyChanged(nameof(ScoreText));
        OnPropertyChanged(nameof(PercentageText));
    }

    private void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}
