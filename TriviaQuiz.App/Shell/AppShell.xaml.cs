using TriviaQuiz.App.Views;

namespace TriviaQuiz.App
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(GameSetupPage), typeof(GameSetupPage));
            Routing.RegisterRoute(nameof(QuizPage), typeof(QuizPage));
            Routing.RegisterRoute(nameof(ResultPage), typeof(ResultPage));
            Routing.RegisterRoute(nameof(StatisticsPage), typeof(StatisticsPage));
        }
    }
}
