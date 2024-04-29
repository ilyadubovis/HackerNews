using Microsoft.AspNetCore.Mvc;
using HackerNewsApp.Server.Models;
using HackerNewsApp.Server.Services;
using Microsoft.AspNetCore.Cors;

namespace HackerNewsApp.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors(PolicyName = "Default")]
public class HackerNewsController(IHackerNewsService hackerNewsService) : ControllerBase
{
    private readonly IHackerNewsService _hackerNewsService = hackerNewsService;

    [HttpGet("{category}")]
    public async Task<ActionResult<IEnumerable<string>>> GetNewsIdsByCategory([FromRoute] string category, [FromQuery] int page) =>
        Ok(await _hackerNewsService.GetNewsIdsByCategory(category, page-1));

    [HttpGet("{category}/pagecount")]
    public async Task<ActionResult<int>> GetPageCountCategory([FromRoute] string category) =>
        Ok(await _hackerNewsService.GetPageCountByCategory(category));

    [HttpGet("story/{id}")]
    public async Task<ActionResult<NewsStory>> GetNewsStoryByid(string id) =>
        Ok(await _hackerNewsService.GetNewsStoryByid(id));
}

