using InstagramWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InstagramWeb.Infrastructure.Data.Configurations;
public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.Property(x => x.Id).IsRequired().HasDefaultValueSql("NEWID()");

        builder.HasOne(x => x.User)
            .WithMany(p => p.Posts).
            HasForeignKey(x => x.UserId).
            OnDelete(DeleteBehavior.Cascade);
    }
}
