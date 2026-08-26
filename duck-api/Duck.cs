namespace DuckPond;

/// <summary>One of the ducks on staff.</summary>
public record Duck(int Id, string Name, string Speciality, string Emoji);

/// <summary>A recorded consultation. These are what have to survive a restart.</summary>
public record Consultation(
    string Id,
    int DuckId,
    string DuckName,
    string DuckEmoji,
    string Problem,
    string Tip,
    DateTimeOffset AskedAt);

/// <summary>What the frontend posts to /api/consultations.</summary>
public record ConsultationRequest(int DuckId, string? Problem);
