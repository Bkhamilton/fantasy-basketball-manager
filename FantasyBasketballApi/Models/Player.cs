namespace FantasyBasketballApi.Models;

public class Player
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Team { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public bool IsStarting { get; set; }
    public PlayerStats? Stats { get; set; }
    public InjuryStatus? Injury { get; set; }
    public GameToday? GameToday { get; set; }
}

public class PlayerStats
{
    public double Points { get; set; }
    public double Rebounds { get; set; }
    public double Assists { get; set; }
    public double Steals { get; set; }
    public double Blocks { get; set; }
    public double FieldGoalPercentage { get; set; }
    public double ThreePointPercentage { get; set; }
}

public class InjuryStatus
{
    public string Status { get; set; } = string.Empty; // "Healthy", "Questionable", "Doubtful", "Out"
    public string? Description { get; set; }
}

public class GameToday
{
    public bool HasGame { get; set; }
    public string? Opponent { get; set; }
    public string? Time { get; set; }
    public bool IsHomeGame { get; set; }
}
