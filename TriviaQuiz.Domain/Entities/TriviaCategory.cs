namespace TriviaQuiz.Domain.Entities;

public sealed class TriviaCategory
{
    public string Key { get; }
    public string Name { get; }

    public TriviaCategory(string key, string name)
    {
        Key = key;
        Name = name;
    }
}
