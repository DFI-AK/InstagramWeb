
namespace InstagramWeb.Domain.Entities;
public class PostImages:BaseEntity<string>
{
    public string? PostId { get; set; }
    public string? ImageUrl { get; set; }

    public Post Post { get; set; } = null!;
}
