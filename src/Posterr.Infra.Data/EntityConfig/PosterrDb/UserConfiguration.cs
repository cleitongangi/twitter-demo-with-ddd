using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Posterr.Domain.Entities;

namespace Posterr.Infra.Data.EntityConfig.PosterrDb
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("Users", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_Users").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("bigint").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Username).HasColumnName(@"Username").HasColumnType("nvarchar(14)").IsRequired().HasMaxLength(14);
            builder.Property(x => x.JoinedAt).HasColumnName(@"JoinedAt").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.FollowersCount).HasColumnName(@"FollowersCount").HasColumnType("int").IsRequired();
            builder.Property(x => x.FollowingCount).HasColumnName(@"FollowingCount").HasColumnType("int").IsRequired();
            builder.Property(x => x.PostsCount).HasColumnName(@"PostsCount").HasColumnType("int").IsRequired();
            builder.Property(x => x.MetricsUpdatedAt).HasColumnName(@"MetricsUpdatedAt").HasColumnType("datetime").IsRequired();

            // Only for test because don't exist Users CRUD
            builder.HasData(
                    new UserEntity
                    {
                        Id = 1,
                        Username = "cleiton.gangi",
                        JoinedAt = DateTime.Now,
                        FollowersCount = 0,
                        FollowingCount = 0,
                        PostsCount = 0,
                        MetricsUpdatedAt = DateTime.Now
                    },
                    new UserEntity
                    {
                        Id = 2,
                        Username = "user2",
                        JoinedAt = DateTime.Now,
                        FollowersCount = 0,
                        FollowingCount = 0,
                        PostsCount = 0,
                        MetricsUpdatedAt = DateTime.Now
                    },
                    new UserEntity
                    {
                        Id = 3,
                        Username = "user3",
                        JoinedAt = DateTime.Now,
                        FollowersCount = 0,
                        FollowingCount = 0,
                        PostsCount = 0,
                        MetricsUpdatedAt = DateTime.Now
                    }
                );
        }
    }
}