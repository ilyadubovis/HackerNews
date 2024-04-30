using HackerNewsApp.Server.Controllers;
using HackerNewsApp.Server.Models;
using HackerNewsApp.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;

namespace HackerNewsServerTest;

[TestClass]
public class NewsServiceTest
{
    private Mock<IHackerNewsService> _hackerNewsService = new();

    [TestInitialize]
    public void Init()
    {
        _hackerNewsService.Setup(x => x.GetPageCountByCategory("topstories"))
            .Returns(Task.FromResult(25));

        _hackerNewsService.Setup(x => x.GetPageCountByCategory(It.IsNotIn<string>(new[] { "topstories" } )))
            .Returns(Task.FromResult(0));

        _hackerNewsService.Setup(x => x.GetNewsIdsByCategory("topstories", It.IsAny<int>()))
            .Returns(Task.FromResult((new[] { "123", "456" } as IEnumerable<string>)));

        _hackerNewsService.Setup(x => x.GetNewsIdsByCategory(It.IsNotIn<string>(new[] { "topstories" }), It.IsAny<int>()))
            .Returns(Task.FromResult(new string[0] as IEnumerable<string>));

        _hackerNewsService.Setup(x => x.GetNewsStoryByid("123"))
            .Returns(Task.FromResult(new NewsStory
            {
                Id = "123",
                Title = "Title",
                Type = "Type",
                By = "By",
                Decscendants = 0,
                Score = 1,
                Time = 12345,
                Url = "http://www.abc.com",
                Kids = null
            }));

        _hackerNewsService.Setup(x => x.GetNewsStoryByid(It.IsNotIn<string>(new[] { "123" })))
            .Returns(Task.FromResult(null as NewsStory));
    }


    [TestMethod]
    public async Task VerifyGetPageCountByValidCategory()
    {
        var hackerNewsController = new HackerNewsController(_hackerNewsService.Object);
        var response = await hackerNewsController.GetPageCountCategory("topstories");

        Assert.IsInstanceOfType<OkObjectResult>(response.Result);

        var pageCount = (response.Result as OkObjectResult)!.Value;
        Assert.AreEqual(pageCount, 25);
    }

    [TestMethod]
    public async Task VerifyGetPageCountByInvalidCategory()
    {
        var hackerNewsController = new HackerNewsController(_hackerNewsService.Object);
        var response = await hackerNewsController.GetPageCountCategory("bogus-category");

        Assert.IsInstanceOfType<OkObjectResult>(response.Result);

        var pageCount = (response.Result as OkObjectResult)!.Value;
        Assert.AreEqual(pageCount, 0);
    }

    [TestMethod]
    public async Task VerifyGetNewsIdsByValidCategory()
    {
        var hackerNewsController = new HackerNewsController(_hackerNewsService.Object);
        var response = await hackerNewsController.GetNewsIdsByCategory("topstories", 1);
        Assert.IsInstanceOfType<OkObjectResult>(response.Result);

        var newsIds = (response.Result as OkObjectResult)!.Value as string[];
        CollectionAssert.AreEqual(newsIds, new[] { "123", "456" });
    }

    [TestMethod]
    public async Task VerifyGetNewsIdsByInvalidCategory()
    {
        var hackerNewsController = new HackerNewsController(_hackerNewsService.Object);
        var response = await hackerNewsController.GetNewsIdsByCategory("bogus-category", 1);
        Assert.IsInstanceOfType<OkObjectResult>(response.Result);

        var newsIds = (response.Result as OkObjectResult)!.Value as string[];
        CollectionAssert.AreEqual(newsIds, new string[0]);
    }

    [TestMethod]
    public async Task VerifyGetNewsStoryByValidId()
    {
        var hackerNewsController = new HackerNewsController(_hackerNewsService.Object);
        var response = await hackerNewsController.GetNewsStoryByid("123");
        Assert.IsInstanceOfType<OkObjectResult>(response.Result);

        var story = (response.Result as OkObjectResult)!.Value as NewsStory;

        Assert.AreEqual(story!.Title, "Title");
        Assert.AreEqual(story!.Type, "Type");
        Assert.AreEqual(story!.Score, 1);
        Assert.AreEqual(story!.Decscendants, 0);
        Assert.AreEqual(story!.By, "By");
        Assert.AreEqual(story!.Time, 12345);
        Assert.AreEqual(story!.Url, "http://www.abc.com");
    }

    [TestMethod]
    public async Task VerifyGetNewsStoryByInvalidId()
    {
        var hackerNewsController = new HackerNewsController(_hackerNewsService.Object);
        var response = await hackerNewsController.GetNewsStoryByid("bogus-id");
        Assert.IsInstanceOfType<OkObjectResult>(response.Result);

        var story = (response.Result as OkObjectResult)!.Value as NewsStory;
        Assert.AreEqual(story, null);
    }
}

