using Microsoft.AspNetCore.Mvc;
using FantasyBasketballApi.Models;
using FantasyBasketballApi.Services;

namespace FantasyBasketballApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalysisController : ControllerBase
{
    private readonly IAnalysisService _analysisService;

    public AnalysisController(IAnalysisService analysisService)
    {
        _analysisService = analysisService;
    }

    [HttpGet("start-sit-recommendations")]
    public ActionResult<List<StartSitRecommendation>> GetStartSitRecommendations()
    {
        return Ok(_analysisService.GetStartSitRecommendations());
    }
}
