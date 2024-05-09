using Moq;
using Moq.Protected;
using System.Text.Json;
using ZumRail_Interview.Applications;
using ZumRail_Interview.Interfaces;
using ZumRail_Interview.Models;

namespace ZumRail_Interview.Tests.ServiceTests;

public class DataProcessingTests
{
    private readonly IDataProcessing _dataProcessing;
    private readonly Mock<IHttpClientFactory> _httpClientFactory;
    private Mock<HttpMessageHandler> handlerMock = new Mock<HttpMessageHandler>();
    public DataProcessingTests()
    {
        _httpClientFactory = new Mock<IHttpClientFactory>();
        handlerMock
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
               "SendAsync",
               ItExpr.IsAny<HttpRequestMessage>(),
               ItExpr.IsAny<CancellationToken>()
           )
           .ReturnsAsync(GetGoodResponseMessage(postList4Record));

        var httpClient = new HttpClient(handlerMock.Object);
        var url = "https://api.hatchways.io/assessment/blog/posts";
        httpClient.BaseAddress = new Uri(url);

        _httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

        _dataProcessing = new DataProcessing(_httpClientFactory.Object);
    }

    [Fact]
    public async Task GetPostListByTag_Returns_WithASCSortedList() 
    {
        var result = await _dataProcessing.GetPostListByTag(["tech"], "reads", "asc");
        Assert.NotNull(result);
        Assert.Equal(4, result.Count);
        Assert.Equal(4, result.First().Id);
        Assert.Equal(2, result.Last().Id);
    }

    [Fact]
    public async Task GetPostListByTag_Returns_WithDESCSortedList() 
    {
        var result = await _dataProcessing.GetPostListByTag(["tech"], "likes", "desc");
        Assert.NotNull(result);
        Assert.Equal(4, result.Count);
        Assert.Equal(1, result.First().Id);
        Assert.Equal(14, result.Last().Id);
    }

    [Fact]
    public async Task GetPostListByTag_Returns_WithUnsuccessful() 
    {
        handlerMock
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
               "SendAsync",
               ItExpr.IsAny<HttpRequestMessage>(),
               ItExpr.IsAny<CancellationToken>()
           )
           .ReturnsAsync(GetBadResponseMessage());

        var exception = await Assert.ThrowsAsync<HttpRequestException>(() =>
            _dataProcessing.GetPostListByTag(new[] { "tech" }, "likes", "desc")
        );
        Assert.Contains("Failed to fetch data", exception.Message);
    }

    [Fact]
    public async Task GetPostListByTag_Returns_WithUnsorted() 
    {
        var result = await _dataProcessing.GetPostListByTag(["tech"], null, null);
        Assert.NotNull(result);
        Assert.Equal(4, result.Count);
        Assert.Equal(1, result.First().Id);
        Assert.Equal(14, result.Last().Id);
    }



    private PostList postList4Record = new PostList()
    {
        Posts = new List<Post>()
            {
                new Post{
                    Author = "Rylee Paul",
                    AuthorId= 9,
                    Id = 1,
                    Likes = 960,
                    Popularity = 0.13,
                    Reads = 50361,
                    Tags = new List<string>{ "tech", "health"}
                },
                new Post{
                    Author = "Zackery Turner",
                    AuthorId= 12,
                    Id = 2,
                    Likes = 469,
                    Popularity = 0.68,
                    Reads = 90406,
                    Tags = new List<string>{ "startups", "tech", "history"}
                },
                new Post{
                    Author = "Elisha Friedman",
                    AuthorId= 8,
                    Id = 4,
                    Likes = 728,
                    Popularity = 0.88,
                    Reads = 19645,
                    Tags = new List<string>{ "startups", "tech", "history"}
                },
                new Post{
                    Author = "Trevon Rodriguez",
                    AuthorId= 5,
                    Id = 14,
                    Likes = 311,
                    Popularity = 0.67,
                    Reads = 25644,
                    Tags = new List<string>{ "startups", "history"}
                },
            }
    };

    private HttpResponseMessage GetGoodResponseMessage(PostList postList) 
    { 
        var postsString = JsonSerializer.Serialize(postList);

        return new HttpResponseMessage()
        {
            Content = new StringContent(postsString),
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }
    private HttpResponseMessage GetBadResponseMessage() 
    { 

        return new HttpResponseMessage()
        {
            Content = new StringContent("Failed to fetch data"),
            StatusCode = System.Net.HttpStatusCode.BadGateway
        };
    }
}
