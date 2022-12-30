using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Posterr.Domain.Entities;

namespace Posterr.Infra.Data.EntityConfig.PosterrDb
{    
    public class PostConfiguration : IEntityTypeConfiguration<PostEntity>
    {
        public void Configure(EntityTypeBuilder<PostEntity> builder)
        {
            builder.ToTable("Posts", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_Posts").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("bigint").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Text).HasColumnName(@"Text").HasColumnType("nvarchar(777)").IsRequired(false).HasMaxLength(777);
            builder.Property(x => x.CreatedAt).HasColumnName(@"CreatedAt").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.UserId).HasColumnName(@"UserId").HasColumnType("bigint").IsRequired();            
            builder.Property(x => x.ParentId).HasColumnName(@"ParentId").HasColumnType("bigint").IsRequired(false);
            builder.Property(x => x.TypeId).HasColumnName(@"TypeId").HasColumnType("int").IsRequired();

            // Foreign keys
            builder.HasOne(a => a.Parent).WithMany(b => b.Posts).HasForeignKey(c => c.ParentId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Posts_PostParent");
            builder.HasOne(a => a.PostType).WithMany(b => b.Posts).HasForeignKey(c => c.TypeId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Posts_PostTypes");
            builder.HasOne(a => a.User).WithMany(b => b.Posts).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Posts_Users");
        }
    }
}