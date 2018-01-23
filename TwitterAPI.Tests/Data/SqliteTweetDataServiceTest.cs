using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
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
            new Tweet() { UserName = "salesforce", CreatedAt = "Sun Jan 21 21:30:07 +0000 2018", ProfileImageUrl = "", RetweetCount = 1, ScreenName = "salesforce", Text = "First in list" },
            new Tweet() { UserName = "salesforce", CreatedAt = "Sun Jan 21 21:30:07 +0000 2018", ProfileImageUrl = "", RetweetCount = 2, ScreenName = "salesforce", Text = "Second in list" }
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
        public async Task GetCurrentTweetsTest()
        {
            var mockSet = new Mock<DbSet<Tweet>>();

            var mockContext = new Mock<FullDbContext>();
            mockContext.Setup(m => m.Set<Tweet>()).Returns(GetQueryableMockDbSet<Tweet>(setupList.ToArray()));

            var service = new SqliteTweetDataService(mockContext.Object);
            await service.UpdateTweets(setupList);

            mockContext.Verify(m => m.SaveChangesAsync(), Times.AtMost(2));
        }
    }
}
