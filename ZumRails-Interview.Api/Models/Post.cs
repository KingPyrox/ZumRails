namespace ZumRail_Interview.Models;

public class Post
{
    public int Id { get; set; }
    public string Author { get; set; }
    public int AuthorId { get; set; }  
    public int Likes { get; set; }
    public double Popularity { get; set; }
    public int Reads { get; set; }
    public List<string> Tags { get; set; } 
}
public class PostList
{ 
    public List<Post> Posts { get; set;}
}
