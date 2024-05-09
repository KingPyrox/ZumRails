using ZumRail_Interview.Models;

namespace ZumRail_Interview.Interfaces;

public interface IDataProcessing
{
    public Task<List<Post>?> GetPostListByTag(string[] tags, string? sortBy, string? direstion);
}
