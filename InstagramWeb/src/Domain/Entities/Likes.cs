namespace InstagramWeb.Domain.Entities;
public class Likes:BaseEntity<string>
{
    public string? PostId { get; set; }
    public string? UserId { get; set;}
    public Post Post { get; set; } = null!;
    public UserProfile User { get; set; } = null!;
}
