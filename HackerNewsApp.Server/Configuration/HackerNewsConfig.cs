namespace HackerNewsApp.Server.Configuration;

public class HackerNewsConfig
{
    public required string ApiEndPoint { get; set; }

    public required string StorySegment { get; set; }

    public required string SuffixSegment { get; set; }

    public required int PageSize { get; set; }
}

