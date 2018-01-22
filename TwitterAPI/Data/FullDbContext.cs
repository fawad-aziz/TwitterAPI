using TwitterAPI.Models.Entities;
using System.Data.Entity;

namespace TwitterAPI.Data
{
    public class FullDbContext : DbContext
    {
        public virtual DbSet<Tweet> Tweets { get; set; }

        public FullDbContext() : base("TweetsContext") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tweet>().ToTable("Tweets");
            modelBuilder.Entity<Tweet>().HasKey(p => p.Id);
            modelBuilder.Entity<Tweet>().Property(p => p.Id).HasColumnName("Id");
            modelBuilder.Entity<Tweet>().Property(p => p.UserName).HasColumnName("UserName");
            modelBuilder.Entity<Tweet>().Property(p => p.ScreenName).HasColumnName("ScreenName");
            modelBuilder.Entity<Tweet>().Property(p => p.ProfileImageUrl).HasColumnName("ProfileImageUrl");
            modelBuilder.Entity<Tweet>().Property(p => p.Text).HasColumnName("Text");
            modelBuilder.Entity<Tweet>().Property(p => p.RetweetCount).HasColumnName("RetweetCount");
            modelBuilder.Entity<Tweet>().Property(p => p.CreatedAt).HasColumnName("CreatedAt");
            modelBuilder.Entity<Tweet>().Property(p => p.IsCurrent).HasColumnName("IsCurrent");
        }
    }
}