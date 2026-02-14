namespace TriviaQuiz.Domain.Entities;

public static class TriviaCategoryRegistry
{
    public static readonly TriviaCategory GeneralKnowledge =
        new("general_knowledge", "General Knowledge");

    public static readonly TriviaCategory Science =
        new("science", "Science");

    public static readonly TriviaCategory Geography =
        new("geography", "Geography");

    public static readonly TriviaCategory History =
        new("history", "History");

    public static readonly TriviaCategory Music =
        new("music", "Music");

    public static readonly TriviaCategory FilmAndTv =
        new("film_and_tv", "Film & TV");

    public static readonly TriviaCategory SportAndLeisure =
        new("sport_and_leisure", "Sport & Leisure");

    public static readonly TriviaCategory ArtsAndLiterature =
        new("arts_and_literature", "Arts & Literature");

    public static readonly TriviaCategory FoodAndDrink =
        new("food_and_drink", "Food & Drink");

    public static readonly TriviaCategory SocietyAndCulture =
        new("society_and_culture", "Society & Culture");

    public static readonly IReadOnlyList<TriviaCategory> All =
    [
        GeneralKnowledge,
        Science,
        Geography,
        History,
        Music,
        FilmAndTv,
        SportAndLeisure,
        ArtsAndLiterature,
        FoodAndDrink,
        SocietyAndCulture
    ];

    private static readonly Dictionary<string, TriviaCategory> ByKey =
        All.ToDictionary(x => x.Key, StringComparer.OrdinalIgnoreCase);

    public static TriviaCategory? FromKey(string key)
    {
        return ByKey.TryGetValue(key, out var cat)
            ? cat
            : null;
    }
}
