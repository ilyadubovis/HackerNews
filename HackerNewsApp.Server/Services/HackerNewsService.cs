using HackerNewsApp.Server.Configuration;
using HackerNewsApp.Server.Models;
using HackerNewsApp.Server.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HackerNewsApp.Server.Services;

public interface IHackerNewsService
{
    public Task<IEnumerable<string>> GetNewsIdsByCategory(string category, int pageIndex);
    
    public Task<int> GetPageCountByCategory([FromRoute] string category);

    public Task<NewsStory?> GetNewsStoryByid(string id);
}

public class HackerNewsService(IHttpClientFactory httpClientFactory, IOptions<HackerNewsConfig> config) : IHackerNewsService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly IOptions<HackerNewsConfig> _config = config;

    public async Task<IEnumerable<string>> GetNewsIdsByCategory(string category, int pageIndex)
    {
        var config = _config.Value;
        var ids = await GetAllNewsIdsByCategory(category);
        return ids.Skip(pageIndex * config.PageSize).Take(config.PageSize);
    }

    public async Task<int> GetPageCountByCategory([FromRoute] string category)
    {
        var ids = await GetAllNewsIdsByCategory(category);
        var config = _config.Value;
        return (int)Math.Ceiling((double)ids.Count() / config.PageSize);
    }

    public async Task<NewsStory?> GetNewsStoryByid(string id)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var config = _config.Value;
        var response = await httpClient.GetAsync($"{config.ApiEndPoint}{config.StorySegment}{id}{config.SuffixSegment}");
        var str = await response.Content.ReadAsStringAsync();
        return str.FromJson<NewsStory>();
    }

    private async Task<IEnumerable<string>> GetAllNewsIdsByCategory(string category)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var config = _config.Value;
        var response = await httpClient.GetAsync($"{config.ApiEndPoint}{category}{config.SuffixSegment}");
        var str = await response.Content.ReadAsStringAsync();
        var ids = str.FromJson<IEnumerable<string>>();
        return ids ?? [];
    }
}
