using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using Xunit.Sdk;
using ZumRail_Interview.Applications;
using ZumRail_Interview.Controllers;
using ZumRail_Interview.Interfaces;
using ZumRail_Interview.Models;

namespace ZumRail_Interview.Tests.ControllerTests;

public class PostControllerTests
{
    private readonly PostsController _controller;
    private readonly Mock<IDataProcessing> _dataProcessing;

    public PostControllerTests()
    {
        _dataProcessing = new Mock<IDataProcessing>();
        _controller = new PostsController(_dataProcessing.Object); 
    }

    [Fact]
    public async Task GetListOfAllPostWithTag_Returns_WithNoIssues()
    {
        var results = await _controller.GetListOfAllPostWithTag("tech,science", "id", "asc");
        Assert.NotNull(results);
    }

    [Fact]
    public async Task GetListOfAllPostWithTag_Returns_WithNoTagsAsync()
    {
        var results = await _controller.GetListOfAllPostWithTag(null, "id", "asc");
        Assert.IsType<BadRequestObjectResult>(results);
    }

    [Fact]
    public async Task GetListOfAllPostWithTag_Returns_WithIncorrectSort()
    {
        var results = await _controller.GetListOfAllPostWithTag("tech,science", "Hi", "asc");
        Assert.IsType<BadRequestObjectResult>(results);
    }

    [Fact]
    public async Task GetListOfAllPostWithTag_Returns_WithIncorrectDirectionAsync()
    {
        var results = await _controller.GetListOfAllPostWithTag("tech,science", "reads", "up");
        Assert.IsType<BadRequestObjectResult>(results);
    }

    [Fact]
    public async Task GetListOfAllPostWithTag_Returns_Null()
    {
        _dataProcessing.Setup(x => x.GetPostListByTag(It.IsAny<string[]>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult<List<Post>?>(null));
        var results = await _controller.GetListOfAllPostWithTag("tech,science", "reads", "asc");
        Assert.IsType<NotFoundObjectResult>(results);
    }
}
