using System.Text.Json.Serialization;

namespace FantasyBasketballApi.Models;

public class SportsDbPlayerResponse
{
    [JsonPropertyName("player")]
    public List<SportsDbPlayer>? Player { get; set; }
}

public class SportsDbPlayer
{
    [JsonPropertyName("idPlayer")]
    public string? IdPlayer { get; set; }

    [JsonPropertyName("strPlayer")]
    public string? StrPlayer { get; set; }

    [JsonPropertyName("strTeam")]
    public string? StrTeam { get; set; }

    [JsonPropertyName("strPosition")]
    public string? StrPosition { get; set; }

    [JsonPropertyName("strHeight")]
    public string? StrHeight { get; set; }

    [JsonPropertyName("strWeight")]
    public string? StrWeight { get; set; }

    [JsonPropertyName("strNumber")]
    public string? StrNumber { get; set; }

    [JsonPropertyName("dateBorn")]
    public string? DateBorn { get; set; }

    [JsonPropertyName("strNationality")]
    public string? StrNationality { get; set; }

    [JsonPropertyName("strThumb")]
    public string? StrThumb { get; set; }
}
