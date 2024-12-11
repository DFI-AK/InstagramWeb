namespace InstagramWeb.Domain.Entities;
public class UserProfile : BaseEntity<string>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public ICollection<Follows> Followers { get; set; } = [];
    public ICollection<Follows> Followed { get; set; } = [];
    public ICollection<UserMessage> SenderMessages { get; set; } = [];
    public ICollection<UserMessage> ReceiverMessage { get; set; } = [];
    public ICollection<Post> Posts { get; set; } = [];
    public ICollection<Likes> Likes { get; set; } = [];
}
