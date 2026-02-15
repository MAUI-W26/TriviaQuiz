using TriviaQuiz.App.ViewModels;

namespace TriviaQuiz.App.Views;

public partial class StatisticsPage : ContentPage
{
    private readonly StatisticsViewModel _viewModel;

    public StatisticsPage(StatisticsViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;

        _viewModel.StatisticsChanged += UpdateUI;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.LoadAsync();
    }

    private void UpdateUI()
    {
        GamesPlayedLabel.Text =
            _viewModel.GamesPlayedText;

        BestScoreLabel.Text =
            _viewModel.BestScoreText;

        AccuracyLabel.Text =
            _viewModel.AccuracyText;
    }

    private void OnResetClicked(
    object sender,
    EventArgs e)
    {
        _viewModel.ResetCommand.Execute(null);
    }

    private void OnBackClicked(
        object sender,
        EventArgs e)
    {
        _viewModel.BackCommand.Execute(null);
    }

}
