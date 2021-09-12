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
        public DbSet<Content> Contents { set; get; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Category>().HasData(
               new Category()
               {
                   CreateAt = DateTime.Now,
                   Id = new Guid("fd039ed9-c9a4-45f9-a4e9-0aa201d0afda"),
                   Name = "Covid",
               });
            builder.Entity<Region>().HasData(
               new Region()
               {
                   CreateAt = DateTime.Now,
                   Id = new Guid("dd176927-f0ff-42eb-8ebd-0846faf35d83"),
                   Name = "Ha Noi",
                   Description = "Khu vuwcj ha noi",
               });
            var hasher = new PasswordHasher<AppUser>();
            builder.Entity<AppUser>().HasData(new AppUser
            {
                Id = new Guid("0027068e-4c5d-4ecb-a157-b9cc063cd672"),
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "huynabhaf190133@fpt.edu.vn",
                NormalizedEmail = "huynabhaf190133@fpt.edu.vn",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "1"),
                SecurityStamp = string.Empty,
                FullName = "Nguyen Anh Huy",
                PhoneNumber = "0399056507",
            });
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
                x.HasOne(x => x.Content).WithMany(x => x.Images).HasForeignKey(x => x.ContentId);
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
            });
            builder.Entity<Content>(x =>
            {
                x.ToTable("Contents");
                x.HasKey(x => x.Id);
                x.HasOne(x => x.Post).WithMany(x => x.Contents).HasForeignKey(x => x.PostId);
            }
         );
        }
    }
}