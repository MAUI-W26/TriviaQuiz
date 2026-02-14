using System.Text.Json.Serialization;

namespace TriviaQuiz.Infrastructure.Trivia.DTOs;

public sealed class OpenTriviaDbResponseDto
{
    [JsonPropertyName("response_code")]
    public int ResponseCode { get; set; }

    [JsonPropertyName("results")]
    public List<OpenTriviaDbQuestionDto> Results { get; set; } = [];
}

public sealed class OpenTriviaDbQuestionDto
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "";

    [JsonPropertyName("difficulty")]
    public string Difficulty { get; set; } = "";

    [JsonPropertyName("category")]
    public string Category { get; set; } = "";

    [JsonPropertyName("question")]
    public string Question { get; set; } = "";

    [JsonPropertyName("correct_answer")]
    public string CorrectAnswer { get; set; } = "";

    [JsonPropertyName("incorrect_answers")]
    public List<string> IncorrectAnswers { get; set; } = [];
}
