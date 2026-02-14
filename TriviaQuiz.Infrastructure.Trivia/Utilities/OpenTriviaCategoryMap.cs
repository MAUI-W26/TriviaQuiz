using TriviaQuiz.Domain.Entities;

namespace TriviaQuiz.Infrastructure.Trivia.Utilities;

public static class OpenTriviaCategoryMap
{
    private static readonly IReadOnlyDictionary<string, int> KeyToId =
        new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            [TriviaCategoryRegistry.GeneralKnowledge.Key] = 9,
            [TriviaCategoryRegistry.FilmAndTv.Key] = 11,
            [TriviaCategoryRegistry.Music.Key] = 12,
            [TriviaCategoryRegistry.Science.Key] = 17,
            [TriviaCategoryRegistry.SportAndLeisure.Key] = 21,
            [TriviaCategoryRegistry.Geography.Key] = 22,
            [TriviaCategoryRegistry.History.Key] = 23,
            [TriviaCategoryRegistry.ArtsAndLiterature.Key] = 25,
            [TriviaCategoryRegistry.SocietyAndCulture.Key] = 26,
            [TriviaCategoryRegistry.FoodAndDrink.Key] = 49,
        };

    public static bool TryGetId(string categoryKey, out int id)
        => KeyToId.TryGetValue(categoryKey, out id);
}
