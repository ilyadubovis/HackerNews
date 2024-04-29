namespace HackerNewsApp.Server.Models;

public class NewsStory
{
    public required string Id { get; set; }

    public required string Title { get; set; }

    public required string Type { get; set; }

    public required int Time { get; set; }

    public required int Score { get; set; }

    public string? Url { get; set; }

    public required string By { get; set; }

    public required int Decscendants { get; set; }

    public string[]? Kids { get; set; }
}

