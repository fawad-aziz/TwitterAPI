using TwitterAPI.Models.Entities;
using System.Data.Entity;
using TwitterAPI.Data.Contracts;
using System;
using System.Linq;
using System.Diagnostics;

namespace TwitterAPI.Data
{
    public class FullDbContext : DbContext, IDataContext
    {
        //public virtual DbSet<Tweet> Tweets { get; set; }

        public FullDbContext() : base("TweetsContext")
        {
            Database.SetInitializer<FullDbContext>(null);
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<Tweet>().ToTable("Tweets");
        //    modelBuilder.Entity<Tweet>().HasKey(p => p.Id);
        //    modelBuilder.Entity<Tweet>().Property(p => p.Id).HasColumnName("Id");
        //    modelBuilder.Entity<Tweet>().Property(p => p.UserName).HasColumnName("UserName");
        //    modelBuilder.Entity<Tweet>().Property(p => p.ScreenName).HasColumnName("ScreenName");
        //    modelBuilder.Entity<Tweet>().Property(p => p.ProfileImageUrl).HasColumnName("ProfileImageUrl");
        //    modelBuilder.Entity<Tweet>().Property(p => p.Text).HasColumnName("Text");
        //    modelBuilder.Entity<Tweet>().Property(p => p.RetweetCount).HasColumnName("RetweetCount");
        //    modelBuilder.Entity<Tweet>().Property(p => p.CreatedAt).HasColumnName("CreatedAt");
        //    modelBuilder.Entity<Tweet>().Property(p => p.IsCurrent).HasColumnName("IsCurrent");
        //}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //TODO: Move this assembly scanning to the reflection helper
            var markerType = typeof(IFullContextDbMapping);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.Contains("TwitterAPI"));
            var mappingTypes = assemblies.SelectMany(
                                            x => x.GetTypes().Where(t => !t.IsInterface && markerType.IsAssignableFrom(t)));

            try
            {
                foreach (var m in mappingTypes)
                {
                    dynamic map = Activator.CreateInstance(m);
                    modelBuilder.Configurations.Add(map);
                }
            }
            catch (Exception ex)
            {
                Trace.Write(ex.Message, nameof(OnModelCreating));
                throw;
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}