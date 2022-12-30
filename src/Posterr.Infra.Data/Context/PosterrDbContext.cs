using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Posterr.Domain.Entities;
using Posterr.Infra.Data.EntityConfig.PosterrDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posterr.Infra.Data.Context
{
    public class PosterrDbContext : DbContext, IPosterrDbContext
    {
        public DbSet<PostEntity> Posts { get; set; } = null!; // Posts
        public DbSet<PostTypeEntity> PostTypes { get; set; } = null!; // PostTypes
        public DbSet<UserEntity> Users { get; set; } = null!; // Users        
        public DbSet<UserFollowingEntity> UserFollowing { get; set; } = null!; // UserFollowing

        public PosterrDbContext(DbContextOptions<PosterrDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new PostTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());            
        }
    }
}