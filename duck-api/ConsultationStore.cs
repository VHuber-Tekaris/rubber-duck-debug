using System.Text.Json;

namespace DuckPond;

/// <summary>
/// The consultation log, kept as a JSON file. Nothing clever - the point is that it
/// is written to a directory inside the container, so it disappears with the
/// container unless that directory is backed by something with a longer life.
/// </summary>
public class ConsultationStore
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    private readonly object _lock = new();
    private readonly string _file;

    public ConsultationStore(IConfiguration configuration, ILogger<ConsultationStore> logger)
    {
        // Default: /app/data inside the container.
        var directory = configuration["DUCKPOND_DATA_DIR"]
                        ?? Path.Combine(AppContext.BaseDirectory, "data");

        Directory.CreateDirectory(directory);
        _file = Path.Combine(directory, "consultations.json");

        logger.LogInformation("Consultation log lives at '{File}'.", _file);
    }

    public List<Consultation> ReadAll()
    {
        lock (_lock)
        {
            if (!File.Exists(_file))
            {
                return [];
            }

            return JsonSerializer.Deserialize<List<Consultation>>(File.ReadAllText(_file)) ?? [];
        }
    }

    public void Append(Consultation consultation)
    {
        lock (_lock)
        {
            var all = ReadAllUnlocked();
            all.Insert(0, consultation);
            File.WriteAllText(_file, JsonSerializer.Serialize(all, JsonOptions));
        }
    }

    private List<Consultation> ReadAllUnlocked()
    {
        if (!File.Exists(_file))
        {
            return [];
        }

        return JsonSerializer.Deserialize<List<Consultation>>(File.ReadAllText(_file)) ?? [];
    }
}
