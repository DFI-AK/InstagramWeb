
namespace InstagramWeb.Domain.Entities;
public class Post : BaseEntity<string>
{
    public string? UserId { get; set; }
    public string? Content { get; set; }
    public PostCatergory Category { get; set; }
    public UserProfile User { get; set; } = null!;

    public ICollection<PostImages> Images { get; set; } = [];
    public ICollection<Likes> Likes { get; set; } = [];
}
