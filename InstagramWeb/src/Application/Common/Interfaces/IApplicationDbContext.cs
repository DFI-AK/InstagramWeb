using InstagramWeb.Domain.Entities;

namespace InstagramWeb.Application.Common.Interfaces;
public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<UserProfile> UserProfiles { get; }

    DbSet<UserMessage> Messages { get; }

    DbSet<Follows> Follows { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
