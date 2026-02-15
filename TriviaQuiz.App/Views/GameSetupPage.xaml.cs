using TriviaQuiz.App.ViewModels;

namespace TriviaQuiz.App.Views;

public partial class GameSetupPage : ContentPage
{
    public GameSetupPage(GameSetupViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}
