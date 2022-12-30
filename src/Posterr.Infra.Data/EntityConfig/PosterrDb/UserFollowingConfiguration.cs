using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Posterr.Domain.Entities;

namespace Extras.MapDatabase.DbMigracaoContas
{
    public class UserFollowingConfiguration : IEntityTypeConfiguration<UserFollowingEntity>
    {
        public void Configure(EntityTypeBuilder<UserFollowingEntity> builder)
        {
            builder.ToTable("UserFollowing", "dbo");
            builder.HasKey(x => x.Id).HasName("PK_UserFollowing").IsClustered();

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("bigint").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.UserId).HasColumnName(@"UserId").HasColumnType("bigint").IsRequired();
            builder.Property(x => x.TargetUserId).HasColumnName(@"TargetUserId").HasColumnType("bigint").IsRequired();
            builder.Property(x => x.CreatedAt).HasColumnName(@"CreatedAt").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.Active).HasColumnName(@"Active").HasColumnType("bit").IsRequired();
            builder.Property(x => x.RemovedAt).HasColumnName(@"RemovedAt").HasColumnType("datetime").IsRequired(false);
        }
    }
}