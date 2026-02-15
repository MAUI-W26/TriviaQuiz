using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using TriviaQuiz.App.Factories;
using TriviaQuiz.Application.Services;
using TriviaQuiz.Domain.Entities;

namespace TriviaQuiz.App.ViewModels;

public sealed class QuizViewModel : INotifyPropertyChanged
{
    private readonly IQuizService _quizService;

    public event PropertyChangedEventHandler? PropertyChanged;

    public event Action<ContentView>? QuestionViewRequested;

    public ICommand NextCommand { get; }

    public bool CanAdvance =>
        _quizService.CanAdvance();

    public string ProgressText =>
        $"{CurrentQuestionIndex + 1} / {TotalQuestionCount}";

    private int CurrentQuestionIndex =>
        _quizService.GetSession().CurrentQuestionIndex;

    private int TotalQuestionCount =>
        _quizService.GetSession().Questions.Count;

    public QuizViewModel(IQuizService quizService)
    {
        _quizService =
            quizService
            ?? throw new ArgumentNullException(nameof(quizService));

        NextCommand =
            new Command(async () =>
                await AdvanceAsync());
    }

    public async Task InitializeAsync()
    {
        var hasSession =
            await _quizService.HasActiveSessionAsync();

        if (!hasSession)
            throw new InvalidOperationException(
                "QuizPage requires an active session.");

        await _quizService.ResumeSessionAsync();

        RenderCurrentQuestion();

        NotifyStateChanged();
    }

    private void RenderCurrentQuestion()
    {
        QuizQuestion question =
            _quizService.GetCurrentQuestion();

        var view =
            QuestionViewFactory.Create(
                question,
                OnAnswerSelected);

        QuestionViewRequested?.Invoke(view);
    }

    private async void OnAnswerSelected(int selectedIndex)
    {
        await _quizService.SelectAnswerAsync(selectedIndex);

        NotifyStateChanged();
    }

    private async Task AdvanceAsync()
    {
        if (!_quizService.CanAdvance())
            return;

        await _quizService.AdvanceAsync();

        if (_quizService.IsCompleted)
        {
            await Shell.Current.GoToAsync(
                nameof(Views.ResultPage));

            return;
        }

        RenderCurrentQuestion();

        NotifyStateChanged();
    }

    private void NotifyStateChanged()
    {
        OnPropertyChanged(nameof(CanAdvance));
        OnPropertyChanged(nameof(ProgressText));
    }

    private void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}
