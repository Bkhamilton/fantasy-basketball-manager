namespace FantasyBasketballApi.Models;

public class NbaGame
{
    public int Id { get; set; }
    public string HomeTeam { get; set; } = string.Empty;
    public string AwayTeam { get; set; } = string.Empty;
    public DateTime GameTime { get; set; }
    public string Status { get; set; } = string.Empty; // "Scheduled", "Live", "Final"
}
