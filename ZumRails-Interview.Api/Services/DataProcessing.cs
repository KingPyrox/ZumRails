using ZumRail_Interview.Interfaces;
using ZumRail_Interview.Models;
using System.Text.Json;

namespace ZumRail_Interview.Applications;

public class DataProcessing: IDataProcessing
{
    private readonly IHttpClientFactory _clientFactory;
    public DataProcessing(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<List<Post>?> GetPostListByTag(string[] tags, string? sortBy, string? direction)
    {
        try
        {
            List<Post> posts = new List<Post>();
            

            var client = _clientFactory.CreateClient("TestClient");
            
            if (tags.Length > 0)
            {
                foreach (var tag in tags)
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, $"?tag={tag}");
                    HttpResponseMessage response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        try
                        {
                            PostList obj = JsonSerializer.Deserialize<PostList>(result, new JsonSerializerOptions()
                            {
                                PropertyNameCaseInsensitive = true
                            });
                            posts.AddRange(obj.Posts);
                        }
                        catch (JsonException ex)
                        {
                            Console.Out.WriteLine($"Error in DataProcessing JsonException: {ex.Message} /n Stack: {ex.StackTrace}");
                            continue; // Continue processing other tags despite the error
                        }
                    }
                    else
                    {
                        throw new HttpRequestException($"Failed to fetch data for tag: {tag}");
                    }
                }
            }

            // Implement IEqualityComparer<Post> to make Distinct work properly
            posts = posts.Distinct().ToList();

            if (!string.IsNullOrEmpty(sortBy))
            {
                var sorter = new Dictionary<string, Func<List<Post>, List<Post>>>
                {
                    ["id"] = p => p.OrderBy(x => x.Id).ToList(),
                    ["likes"] = p => p.OrderBy(x => x.Likes).ToList(),
                    ["reads"] = p => p.OrderBy(x => x.Reads).ToList(),
                    ["popularity"] = p => p.OrderBy(x => x.Popularity).ToList(),
                };

                var descSorter = new Dictionary<string, Func<List<Post>, List<Post>>>
                {
                    ["id"] = p => p.OrderByDescending(x => x.Id).ToList(),
                    ["likes"] = p => p.OrderByDescending(x => x.Likes).ToList(),
                    ["reads"] = p => p.OrderByDescending(x => x.Reads).ToList(),
                    ["popularity"] = p => p.OrderByDescending(x => x.Popularity).ToList(),
                };
                //Checks direction for which dictoionary to sort with
                posts = (direction == "desc" ? descSorter : sorter).GetValueOrDefault(sortBy, p => p)(posts);
            }
            return posts;
        } catch (Exception ex) {
            Console.Out.WriteLine($"Error in DataProcessing: {ex.Message} /n Stack: {ex.StackTrace}");
            throw;
        }
    }
}
