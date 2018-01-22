namespace TwitterAPI.Models.Entities
{
    public class Tweet
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string ScreenName { get; set; }

        public string ProfileImageUrl { get; set; }

        public string Text { get; set; }

        public int RetweetCount { get; set; }

        public string CreatedAt { get; set; }

        public bool IsCurrent { get; set; }
    }
}