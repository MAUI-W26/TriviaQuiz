namespace TriviaQuiz.Domain.Entities;

public sealed class AnswerOption // sealed -> prevent inheritance, as this class is not intended to be a base class
{
    // required -> ensures that this property must be set during object initialization. eg: var option = new AnswerOption { Text = "Option A", IsCorrect = true };
    public required string Text { get; init; } 
    public bool IsCorrect { get; init; }
}
