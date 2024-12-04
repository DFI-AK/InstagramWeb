using InstagramWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InstagramWeb.Infrastructure.Data.Configurations;
public class FollowsConfiguration : IEntityTypeConfiguration<Follows>
{
    public void Configure(EntityTypeBuilder<Follows> builder)
    {
        builder.HasKey(x => new { x.FollowerId, x.FollowedId });

        builder.HasOne(x => x.Followed)
            .WithMany(x => x.Followed)
            .HasForeignKey(x => x.FollowedId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Follower)
            .WithMany(x => x.Followers)
            .HasForeignKey(x => x.FollowerId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
