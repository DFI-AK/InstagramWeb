using InstagramWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InstagramWeb.Infrastructure.Data.Configurations;
public class PostImagesConfiguration : IEntityTypeConfiguration<PostImages>
{
    public void Configure(EntityTypeBuilder<PostImages> builder)
    {
        builder.Property(x => x.Id).IsRequired().HasDefaultValueSql("NEWID()");
        builder.HasOne(x=>x.Post).
            WithMany(x => x.Images)
            .HasForeignKey(x=>x.PostId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
