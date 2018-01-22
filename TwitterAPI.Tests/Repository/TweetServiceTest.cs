using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterAPI.Data.Contracts;
using TwitterAPI.Models.Entities;
using TwitterAPI.Models.Projections;
using TwitterAPI.Repository.Concrete;

namespace TwitterAPI.Tests.Repository
{
    [TestClass]
    public class TweetServiceTest
    {
        List<Tweet> setupList = new List<Tweet>()
        {
            new Tweet() { UserName = "salesforce", CreatedAt = "Sun Jan 21 21:30:07 +0000 2018", ProfileImageUrl = "", RetweetCount = 1, ScreenName = "salesforce", Text = "First in list" },
            new Tweet() { UserName = "salesforce", CreatedAt = "Sun Jan 21 21:30:07 +0000 2018", ProfileImageUrl = "", RetweetCount = 2, ScreenName = "salesforce", Text = "Second in list" }
        };

        private TweetService GetService(List<Tweet> tweetList = null)
        {
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<List<TweetProjection>>(It.IsAny<List<Tweet>>()))
                .Returns((List<Tweet> source) =>
                {
                    var projections = new List<TweetProjection>();
                    foreach (var tweet in source)
                    {
                        var projection = new TweetProjection()
                        {
                            UserName = tweet.UserName,
                            ScreenName = tweet.ScreenName,
                            CreatedAt = tweet.CreatedAt,
                            ProfileImageUrl = tweet.ProfileImageUrl,
                            RetweetCount = tweet.RetweetCount,
                            Text = tweet.Text
                        };
                        projections.Add(projection);
                    }
                    return projections;
                });

            var tweetDataService = new Mock<ITweetDataService>();
            if (tweetList == null)
            {
                tweetDataService.Setup(t => t.GetCurrentTweets()).Returns(Task.FromResult(new List<Tweet>()));
            }
            else
            {
                tweetDataService.Setup(t => t.GetCurrentTweets()).Returns(Task.FromResult(tweetList));
            }

            var tweetService = new TweetService(mockMapper.Object, tweetDataService.Object);
            return tweetService;
        }

        [TestMethod]
        public async Task GetTweetsTypeTest()
        {
            var tweetService = GetService();
            var tweets = await tweetService.GetTweets();
            Assert.IsInstanceOfType(tweets, typeof(List<TweetProjection>));
        }

        [TestMethod]
        public async Task GetTweetsReturnTest()
        {
            var tweetService = GetService(setupList);
            var tweets = await tweetService.GetTweets();
            Assert.AreEqual(2, tweets.Count);
            Assert.AreEqual(2, tweets[1].RetweetCount);
        }
    }
}
