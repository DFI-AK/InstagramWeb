using InstagramWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InstagramWeb.Infrastructure.Data.Configurations;
internal class LikesConfiguration : IEntityTypeConfiguration<Likes>
{
    public void Configure(EntityTypeBuilder<Likes> builder)
    {
        builder.Property(x => x.Id).IsRequired().HasDefaultValueSql("NEWID()");
        builder.HasOne(x=>x.Post).WithMany(p=>p.Likes).HasForeignKey(x=>x.PostId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x=>x.User).WithMany(p=>p.Likes).HasForeignKey(x=>x.UserId).OnDelete(DeleteBehavior.NoAction);
    }
}
