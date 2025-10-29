using FantasyBasketballApi.Models;

namespace FantasyBasketballApi.Services;

public interface IAnalysisService
{
    List<StartSitRecommendation> GetStartSitRecommendations();
}

public class AnalysisService : IAnalysisService
{
    private readonly IPlayerService _playerService;

    public AnalysisService(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    public List<StartSitRecommendation> GetStartSitRecommendations()
    {
        var players = _playerService.GetAllPlayers();
        var recommendations = new List<StartSitRecommendation>();

        foreach (var player in players)
        {
            var recommendation = AnalyzePlayer(player);
            recommendations.Add(recommendation);
        }

        return recommendations.OrderByDescending(r => r.ConfidenceScore).ToList();
    }

    private StartSitRecommendation AnalyzePlayer(Player player)
    {
        var reasons = new List<string>();
        double score = 50.0; // Base score

        // Check if player has a game today
        if (player.GameToday?.HasGame != true)
        {
            score -= 40;
            reasons.Add("No game scheduled today");
        }
        else
        {
            score += 20;
            reasons.Add($"Playing vs {player.GameToday.Opponent} at {player.GameToday.Time}");
        }

        // Check injury status
        if (player.Injury != null)
        {
            switch (player.Injury.Status.ToLower())
            {
                case "healthy":
                    score += 20;
                    reasons.Add("Fully healthy");
                    break;
                case "questionable":
                    score -= 15;
                    reasons.Add($"Questionable: {player.Injury.Description}");
                    break;
                case "doubtful":
                    score -= 30;
                    reasons.Add($"Doubtful: {player.Injury.Description}");
                    break;
                case "out":
                    score -= 50;
                    reasons.Add($"Out: {player.Injury.Description}");
                    break;
            }
        }

        // Analyze stats performance
        if (player.Stats != null)
        {
            if (player.Stats.Points > 25)
            {
                score += 10;
                reasons.Add($"Strong scorer ({player.Stats.Points:F1} PPG)");
            }

            if (player.Stats.FieldGoalPercentage > 50)
            {
                score += 5;
                reasons.Add($"Efficient shooter ({player.Stats.FieldGoalPercentage:F1}% FG)");
            }

            // All-around performance
            if (player.Stats.Rebounds > 7 && player.Stats.Assists > 5)
            {
                score += 10;
                reasons.Add("Well-rounded stat line");
            }
        }

        // Cap score between 0 and 100
        score = Math.Max(0, Math.Min(100, score));

        return new StartSitRecommendation
        {
            PlayerId = player.Id,
            PlayerName = player.Name,
            Recommendation = score >= 50 ? "Start" : "Sit",
            ConfidenceScore = score,
            Reasons = reasons
        };
    }
}
