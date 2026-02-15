using TriviaQuiz.App.ViewModels;

namespace TriviaQuiz.App.Views;

public partial class MainMenuPage : ContentPage
{
    public MainMenuPage(MainMenuViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}