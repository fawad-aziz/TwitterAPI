using System.Data.Entity.ModelConfiguration;
using TwitterAPI.Data.Contracts;
using TwitterAPI.Models.Entities;

namespace TwitterAPI.Data.Maps
{
    public class TweetMap : EntityTypeConfiguration<Tweet>, IFullContextDbMapping
    {
        public TweetMap()
        {
            ToTable("Tweets");

            HasKey(p => p.Id);

            Property(p => p.Id).HasColumnName("Id");
            Property(p => p.UserName).HasColumnName("UserName");
            Property(p => p.ScreenName).HasColumnName("ScreenName");
            Property(p => p.ProfileImageUrl).HasColumnName("ProfileImageUrl");
            Property(p => p.Text).HasColumnName("Text");
            Property(p => p.RetweetCount).HasColumnName("RetweetCount");
            Property(p => p.CreatedAt).HasColumnName("CreatedAt");
            Property(p => p.IsCurrent).HasColumnName("IsCurrent");
        }
    }
}