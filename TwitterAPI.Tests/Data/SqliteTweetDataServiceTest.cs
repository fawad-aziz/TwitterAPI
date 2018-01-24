using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using TwitterAPI.Data;
using TwitterAPI.Data.Concrete;
using TwitterAPI.Models.Entities;

namespace TwitterAPI.Tests.Data
{
    [TestClass]
    public class SqliteTweetDataServiceTest
    {
        List<Tweet> setupList = new List<Tweet>()
        {
            new Tweet() { UserName = "salesforce", CreatedAt = "Sun Jan 21 21:30:07 +0000 2018", ProfileImageUrl = "", RetweetCount = 1, ScreenName = "salesforce", Text = "First in list", IsCurrent = true },
            new Tweet() { UserName = "salesforce", CreatedAt = "Sun Jan 21 21:30:07 +0000 2018", ProfileImageUrl = "", RetweetCount = 2, ScreenName = "salesforce", Text = "Second in list", IsCurrent = false }
        };

        private static DbSet<T> GetQueryableMockDbSet<T>(params T[] sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return dbSet.Object;
        }

        [TestMethod]
        public async Task UpdateTweetsTest()
        {
            var mockSet = new Mock<DbSet<Tweet>>();

            var mockContext = new Mock<FullDbContext>();
            mockContext.Setup(m => m.Set<Tweet>()).Returns(GetQueryableMockDbSet<Tweet>(setupList.ToArray()));

            var service = new SqliteTweetDataService(mockContext.Object);
            await service.UpdateTweets(setupList);

            mockContext.Verify(m => m.SaveChangesAsync(), Times.AtMost(2));
        }

        [TestMethod]
        public async Task GetCurrentTweetsTest()
        {

            var data = setupList.AsQueryable();

            var mockSet = new Mock<DbSet<Tweet>>();
            mockSet.As<IDbAsyncEnumerable<Tweet>>().Setup(m => m.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<Tweet>(data.GetEnumerator()));

            mockSet.As<IQueryable<Tweet>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<Tweet>(data.Provider));

            mockSet.As<IQueryable<Tweet>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Tweet>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Tweet>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<FullDbContext>();
            mockContext.Setup(c => c.Set<Tweet>()).Returns(mockSet.Object);

            var service = new SqliteTweetDataService(mockContext.Object);
            var tweets = await service.GetCurrentTweets();

            Assert.AreEqual(1, tweets.Count);
        }
    }
}
