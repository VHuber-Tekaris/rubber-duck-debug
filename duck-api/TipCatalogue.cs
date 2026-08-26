using System.Text.Json;

namespace DuckPond;

/// <summary>
/// Loads the tip catalogue from disk. The path is configurable so the same image can
/// be pointed at a different catalogue. If the file is not there the api keeps
/// working with a single built-in tip - and says so in the log.
/// </summary>
public class TipCatalogue
{
    private const string FallbackTip = "Have you tried explaining it to me line by line?";

    private readonly Dictionary<int, string[]> _tips;

    public TipCatalogue(IConfiguration configuration, ILogger<TipCatalogue> logger)
    {
        // Default: next to the assembly, which is /app inside the container.
        var path = configuration["DUCKPOND_TIPS_FILE"]
                   ?? Path.Combine(AppContext.BaseDirectory, "duck-tips.json");

        if (!File.Exists(path))
        {
            logger.LogWarning(
                "Tip catalogue not found at '{Path}'. Falling back to 1 built-in tip, " +
                "so every duck will say the same thing.", path);
            _tips = [];
            return;
        }

        var json = File.ReadAllText(path);
        _tips = JsonSerializer.Deserialize<Dictionary<string, string[]>>(json)
                    ?.ToDictionary(entry => int.Parse(entry.Key), entry => entry.Value)
                ?? [];

        logger.LogInformation("Loaded tips for {Count} ducks from '{Path}'.", _tips.Count, path);
    }

    public string TipFor(Duck duck)
    {
        if (!_tips.TryGetValue(duck.Id, out var tips) || tips.Length == 0)
        {
            return FallbackTip;
        }

        return tips[Random.Shared.Next(tips.Length)];
    }
}
