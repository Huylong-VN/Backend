using Backend.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Backend.Data.DbContext
{
    public class NewsAppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public NewsAppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Region> Regions { set; get; }
        public DbSet<Post> Posts { set; get; }
        public DbSet<Category> Categories { set; get; }
        public DbSet<Image> Images { set; get; }
        public DbSet<PostRegion> PostRegions { set; get; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //IdentityUserLogin
            builder.Entity<IdentityUserLogin<Guid>>().HasKey(x => x.UserId);
            //IdentityUserRole
            builder.Entity<IdentityUserRole<Guid>>().HasKey(x => new { x.UserId, x.RoleId });
            //IdentityUserToken
            builder.Entity<IdentityUserToken<Guid>>().HasKey(x => x.UserId);
            //Post
            builder.Entity<Post>(x =>
            {
                x.ToTable("Posts");
                x.HasKey(x => x.Id);
                x.Property(x => x.Title).IsRequired();
                x.Property(x => x.Content).IsRequired();
                x.HasOne(x => x.Category).WithMany(x => x.Posts).HasForeignKey(x => x.CategoryId);
            }
            );
            //Category
            builder.Entity<Category>(x =>
            {
                x.ToTable("Categories");
                x.HasKey(x => x.Id);
                x.Property(x => x.Name).IsRequired();
            });
            //Image
            builder.Entity<Image>(x =>
            {
                x.ToTable("Images");
                x.HasKey(x => x.Id);
                x.Property(x => x.Path).IsRequired();
                x.HasOne(x => x.Post).WithMany(x => x.Images).HasForeignKey(x => x.PostId);
            });
            //Region
            builder.Entity<Region>(x =>
            {
                x.ToTable("Regions");
                x.HasKey(x => x.Id);
                x.Property(x => x.Name).IsRequired();
                x.Property(x => x.Description).IsRequired();
            });
            //PostRegion
            builder.Entity<PostRegion>(x =>
            {
                x.ToTable("PostRegion");
                x.HasKey(x => new { x.PostId, x.RegionId });
                x.HasOne(x => x.Post).WithMany(x => x.PostRegions).HasForeignKey(x => x.PostId);
                x.HasOne(x => x.Region).WithMany(x => x.PostRegions).HasForeignKey(x => x.RegionId);
            }
         );
        }
    }
}