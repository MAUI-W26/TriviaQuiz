using TriviaQuiz.App.ViewModels;

namespace TriviaQuiz.App.Views;

public partial class QuizPage : ContentPage
{
    private readonly QuizViewModel _viewModel;

    public QuizPage(QuizViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;

        BindingContext = _viewModel;

        _viewModel.QuestionViewRequested += OnQuestionViewRequested;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.InitializeAsync();
    }

    private void OnQuestionViewRequested(ContentView questionView)
    {
        QuestionHost.Content = questionView;
    }
}
