using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NutriTrackData.Entities;

namespace NutriTrack.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<MealCategory> MealCategories { get; set; }
        public DbSet<MealProduct> MealProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PhysicalActivity> PhysicalActivities { get; set; }
        public DbSet<WeightHistory> WeightHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<WeightHistory>()
                .HasOne(wh => wh.User)
                .WithMany(u => u.WeightHistory)
                .HasForeignKey(wh => wh.UserId);

            builder.Entity<User>()
            .HasMany(u => u.Meals)
            .WithOne(m => m.User)
            .HasForeignKey(m => m.UserId);

            builder.Entity<User>()
                .HasMany(p => p.PhysicalActivities)
                .WithOne(pa => pa.User)
                .HasForeignKey(pa => pa.UserId);

            builder.Entity<Meal>()
                .HasOne(m => m.Category)
                .WithMany(c => c.Meals)
                .HasForeignKey(m => m.CategoryId);

            builder.Entity<MealProduct>()
                .HasKey(mp => new { mp.MealId, mp.ProductId });

            builder.Entity<MealProduct>()
                .HasOne(mp => mp.Meal)
                .WithMany(m => m.MealProducts)
                .HasForeignKey(mp => mp.MealId);

            builder.Entity<MealProduct>()
                .HasOne(mp => mp.Product)
                .WithMany(p => p.MealProducts)
                .HasForeignKey(mp => mp.ProductId);

            builder.Entity<MealCategory>()
                .HasData(
                    new MealCategory { CategoryId = 1, Name = "Breakfast" },
                    new MealCategory { CategoryId = 2, Name = "Lunch" },
                    new MealCategory { CategoryId = 3, Name = "Dinner" },
                    new MealCategory { CategoryId = 4, Name = "Snack" }
                );
        }
    }
}

