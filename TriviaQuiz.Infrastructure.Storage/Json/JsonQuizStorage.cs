using System.Text.Json;
using TriviaQuiz.Domain.Contracts;
using TriviaQuiz.Domain.Entities;

namespace TriviaQuiz.Infrastructure.Storage.Json;

public sealed class JsonQuizStorage : IQuizStorage
{
    private readonly string _sessionPath;
    private readonly string _statisticsPath;

    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web);

    public JsonQuizStorage(string baseFolder)
    {
        Directory.CreateDirectory(baseFolder);

        _sessionPath = Path.Combine(baseFolder, "session.json");
        _statisticsPath = Path.Combine(baseFolder, "statistics.json");
    }

    public async Task<QuizSession?> LoadSessionAsync(
        CancellationToken cancellationToken = default)
    {
        if (!File.Exists(_sessionPath))
            return null;

        var json = await File.ReadAllTextAsync(_sessionPath, cancellationToken);

        return JsonSerializer.Deserialize<QuizSession>(
            json,
            JsonOptions);
    }

    public async Task SaveSessionAsync(
        QuizSession session,
        CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(session, JsonOptions);

        await File.WriteAllTextAsync(
            _sessionPath,
            json,
            cancellationToken);
    }

    public Task DeleteSessionAsync(
        CancellationToken cancellationToken = default)
    {
        if (File.Exists(_sessionPath))
            File.Delete(_sessionPath);

        return Task.CompletedTask;
    }

    public async Task<QuizStatistics?> LoadStatisticsAsync(
        CancellationToken cancellationToken = default)
    {
        if (!File.Exists(_statisticsPath))
            return null;

        var json = await File.ReadAllTextAsync(
            _statisticsPath,
            cancellationToken);

        return JsonSerializer.Deserialize<QuizStatistics>(
            json,
            JsonOptions);
    }

    public async Task SaveStatisticsAsync(
        QuizStatistics statistics,
        CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(statistics, JsonOptions);

        await File.WriteAllTextAsync(
            _statisticsPath,
            json,
            cancellationToken);
    }
}
