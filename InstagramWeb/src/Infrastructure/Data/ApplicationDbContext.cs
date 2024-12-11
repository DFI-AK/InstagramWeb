using System.Reflection;
using InstagramWeb.Application.Common.Interfaces;
using InstagramWeb.Domain.Entities;
using InstagramWeb.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InstagramWeb.Infrastructure.Data;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<TodoList> TodoLists => Set<TodoList>();

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

    public DbSet<UserMessage> Messages => Set<UserMessage>();

    public DbSet<Follows> Follows => Set<Follows>();

    public DbSet<Post> Posts =>  Set<Post>();

    public DbSet<PostImages> PostImages =>  Set<PostImages>();

    public DbSet<Likes> Likes =>  Set<Likes>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
