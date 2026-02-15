using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Storage;

using TriviaQuiz.Application.Services;
using TriviaQuiz.Domain.Contracts;

using TriviaQuiz.Infrastructure.Trivia.Services;

using TriviaQuiz.Infrastructure.Storage.SQLite;
using TriviaQuiz.Infrastructure.Storage.Json;
using TriviaQuiz.Infrastructure.Storage.Services;

using TriviaQuiz.App.ViewModels;
using TriviaQuiz.App.Views;

namespace TriviaQuiz.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var services = builder.Services;

        services.AddSingleton<HttpClient>();

        var appDataFolder = FileSystem.AppDataDirectory;

        var dbPath = Path.Combine(
            appDataFolder,
            "triviaquiz.db");

        services.AddSingleton(
            new SQLiteConnectionFactory(dbPath));

        services.AddSingleton<SQLiteQuizStorage>();

        services.AddSingleton<JsonQuizStorage>(serviceProvider =>
            new JsonQuizStorage(appDataFolder));

        services.AddSingleton<IQuizStorage>(serviceProvider =>
        {
            var primary =
                serviceProvider.GetRequiredService<SQLiteQuizStorage>();

            var fallback =
                serviceProvider.GetRequiredService<JsonQuizStorage>();

            return new QuizStorageFacade(
                primary,
                fallback);
        });


        services.AddSingleton<IQuizQuestionService,
            QuizQuestionService>();

        services.AddSingleton<IQuizService,
            QuizService>();

        services.AddTransient<MainMenuViewModel>();
        services.AddTransient<GameSetupViewModel>();
        services.AddTransient<QuizViewModel>();
        services.AddTransient<ResultViewModel>();
        services.AddTransient<StatisticsViewModel>();

        services.AddTransient<MainMenuPage>();
        services.AddTransient<GameSetupPage>();
        services.AddTransient<QuizPage>();
        services.AddTransient<ResultPage>();
        services.AddTransient<StatisticsPage>();

        return builder.Build();
    }
}
