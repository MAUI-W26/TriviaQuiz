using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Extensions.Logging;

using TriviaQuiz.Application.Services;
using TriviaQuiz.Domain.Contracts;

using TriviaQuiz.Infrastructure.Trivia.Services;
using TriviaQuiz.Infrastructure.Storage.Services;
using Microsoft.Extensions.DependencyInjection;


namespace TriviaQuiz.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.Services.AddSingleton<HttpClient>();

        builder.Services.AddSingleton<IQuizStorage, QuizStorageFacade>();

        builder.Services.AddSingleton<IQuizQuestionService, QuizQuestionService>();
        builder.Services.AddSingleton<ITriviaCatalogService, TriviaCatalogService>();

        builder.Services.AddSingleton<IQuizService, QuizService>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
