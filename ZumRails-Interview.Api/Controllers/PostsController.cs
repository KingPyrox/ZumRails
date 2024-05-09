using Microsoft.AspNetCore.Mvc;
using ZumRail_Interview.Applications;
using ZumRail_Interview.Interfaces;

namespace ZumRail_Interview.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : Controller
{
    private readonly IDataProcessing _dp;
    public PostsController(IDataProcessing processing)
    {
        _dp = processing;
    }

    [HttpGet]
    public async Task<IActionResult> GetListOfAllPostWithTag([FromQuery(Name = "tags")] string tags, [FromQuery(Name = "sortBy")] string? sortBy, [FromQuery(Name = "direction")] string? direction)
    {

        if (tags == null || tags == string.Empty)
            return BadRequest("tags parameter is required");

        string[] TagList = tags.Trim().Split(',');

        if(!string.IsNullOrEmpty(sortBy))
            if (!sortBy.Equals("id") &&
                !sortBy.Equals("likes") &&
                !sortBy.Equals("reads") &&
                !sortBy.Equals("popularity"))
                return BadRequest("sortBy parameter is invalid");
        
        if(!string.IsNullOrEmpty(direction))
            if (!direction.Equals("asc") &&
                !direction.Equals("desc"))
                return BadRequest("direction parameter is invalid");


        var results = await _dp.GetPostListByTag(TagList, sortBy, direction);

        if (results != null)
            return Ok(results);
        else
            return NotFound("The posts with the tags you've inputted are not found. PLease try again");
    }
}
