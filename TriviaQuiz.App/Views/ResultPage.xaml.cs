using TriviaQuiz.App.ViewModels;

namespace TriviaQuiz.App.Views;

public partial class ResultPage : ContentPage
{
    private readonly ResultViewModel _viewModel;

    public ResultPage(ResultViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.InitializeAsync();
    }
}
