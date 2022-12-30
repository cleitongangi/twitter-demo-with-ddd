using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Posterr.Domain.Entities;

namespace Posterr.Infra.Data.EntityConfig.PosterrDb
{
    public class PostTypeConfiguration : IEntityTypeConfiguration<PostTypeEntity>
    {
        public void Configure(EntityTypeBuilder<PostTypeEntity> builder)
        {
            builder.ToTable("PostTypes", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_PostTypes").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedNever();
            builder.Property(x => x.TypeDescription).HasColumnName(@"TypeDescription").HasColumnType("varchar(6)").IsRequired(false).IsUnicode(false).HasMaxLength(6);

            builder.HasData(
                    new PostTypeEntity
                    {
                        Id = 1,
                        TypeDescription = "Post"
                    },
                    new PostTypeEntity
                    {
                        Id = 2,
                        TypeDescription = "Repost"
                    },
                    new PostTypeEntity
                    {
                        Id = 3,
                        TypeDescription = "Quote"
                    }
                );
        }
    }
}