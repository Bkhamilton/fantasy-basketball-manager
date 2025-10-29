namespace FantasyBasketballApi.Models;

public class StartSitRecommendation
{
    public int PlayerId { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public string Recommendation { get; set; } = string.Empty; // "Start" or "Sit"
    public double ConfidenceScore { get; set; } // 0-100
    public List<string> Reasons { get; set; } = new();
}
