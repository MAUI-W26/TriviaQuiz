using System.Text.Json.Serialization;

namespace TriviaQuiz.Infrastructure.Trivia.DTOs;

public sealed class TriviaApiQuestionDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("category")]
    public string Category { get; set; } = "";

    [JsonPropertyName("difficulty")]
    public string Difficulty { get; set; } = "";

    [JsonPropertyName("type")]
    public string Type { get; set; } = "";

    [JsonPropertyName("question")]
    public TriviaApiQuestionTextDto Question { get; set; } = new();

    [JsonPropertyName("correctAnswer")]
    public string CorrectAnswer { get; set; } = "";

    [JsonPropertyName("incorrectAnswers")]
    public List<string> IncorrectAnswers { get; set; } = [];
}

public sealed class TriviaApiQuestionTextDto  // because question is an object with a "text" property, not a simple string
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = "";
}
