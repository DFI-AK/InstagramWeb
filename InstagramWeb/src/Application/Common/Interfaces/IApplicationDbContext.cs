using InstagramWeb.Domain.Entities;

namespace InstagramWeb.Application.Common.Interfaces;
public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<UserProfile> UserProfiles { get; }

    DbSet<UserMessage> Messages { get; }

    DbSet<Follows> Follows { get; }
    DbSet<Post> Posts { get; }
    DbSet<PostImages> PostImages { get; }
    DbSet<Likes> Likes { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
