using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwitterSample.Hubs;
using TwitterSample.Services;
using TwitterSample.Tests.Utils;

namespace TwitterSample.Tests
{
    [TestClass]
    public class TwitterStreamServiceTests
    {
        protected static TwitterStreamService GetTestTwitterStreamService(Mock<ITwitterService> mockTwitterService = null, Mock<IHubConnectionContext> mockHubConnectionContext = null)
        {
            if (mockTwitterService == null)
            {
                mockTwitterService = new Mock<ITwitterService>();
            }
            if (mockHubConnectionContext == null)
            {
                mockHubConnectionContext = new Mock<IHubConnectionContext>();
            }

            var sut = new TwitterStreamService(mockTwitterService.Object, mockHubConnectionContext.Object);
            return sut;
        }

        protected static Tweet GetTestTweet()
        {
            return new Tweet(DateTime.Now);
        }

        [TestClass]
        public class TheConstructor
        {
            [TestMethod]
            public void GivenNullParameters_ItShouldThrow()
            {
                ContractAssert.ThrowsArgNull(() => new TwitterStreamService(null, new Mock<IHubConnectionContext>().Object), "twitterService");
                ContractAssert.ThrowsArgNull(() => new TwitterStreamService(new Mock<ITwitterService>().Object, null), "clients");            
            }
        }

        [TestClass]
        public class TheGetTweetsByIdAsyncMethod
        {
            [TestMethod]
            public async Task WillCacheTweetsIfTheyHaveAlreadyBeenRetrieved()
            {
                // Arrange
                var mockTwitterService = new Mock<ITwitterService>();
                var twitterIds = new List<string> { "@test1", "@test2" };
                var tweets = new List<Tweet>() { GetTestTweet(), GetTestTweet() };

                foreach (var twitterId in twitterIds)
                {
                    var id = twitterId;
                    mockTwitterService.Setup(x => x.GetTimeLineByIdAsync(id)).ReturnsAsync(tweets);
                }

                var sut = GetTestTwitterStreamService(mockTwitterService, null);

                // Act 
                await sut.GetTweetsByIdAsync(twitterIds);

                await sut.GetTweetsByIdAsync(twitterIds);

                // Assert
                mockTwitterService.Verify(x => x.GetTimeLineByIdAsync(twitterIds[0]), Times.Once());
                mockTwitterService.Verify(x => x.GetTimeLineByIdAsync(twitterIds[1]), Times.Once());
            }

            [TestMethod]
            public async Task WillMapTweetsToAViewModel()
            {
                // Arrange
                var mockTwitterService = new Mock<ITwitterService>();
                var twitterIds = new List<string> { "@test1", "@test2" };
                var tweets1 = new List<Tweet>() { GetTestTweet(), GetTestTweet() };
                var tweets2 = new List<Tweet>() { GetTestTweet(), GetTestTweet() };

                mockTwitterService.Setup(x => x.GetTimeLineByIdAsync(twitterIds[0])).ReturnsAsync(tweets1);
                mockTwitterService.Setup(x => x.GetTimeLineByIdAsync(twitterIds[1])).ReturnsAsync(tweets2);

                var sut = GetTestTwitterStreamService(mockTwitterService, null);

                // Act 
                var tweetStreamViewModel = await sut.GetTweetsByIdAsync(twitterIds);

                // Assert
                Assert.IsTrue(tweetStreamViewModel.Count == 2, "Expected 2 ViewModels for 2 Unique IDs");
            }

            [TestMethod]
            public async Task WillReturnAViewModelThatIsSortedByTime()
            {
                // Arrange
                var mockTwitterService = new Mock<ITwitterService>();
                var twitterIds = new List<string> { "@test1" };
                var now = DateTime.Now;
                var twoDaysAgo = DateTime.Now.AddDays(-2);
                var threeDaysAgo = DateTime.Now.AddDays(-3);

                var tweets = new List<Tweet>() { 
                    new Tweet(twoDaysAgo), 
                    new Tweet(threeDaysAgo), 
                    new Tweet(now) 
                };
                
                mockTwitterService.Setup(x => x.GetTimeLineByIdAsync(twitterIds[0])).ReturnsAsync(tweets);
              
                var sut = GetTestTwitterStreamService(mockTwitterService, null);

                // Act 
                var tweetStreamViewModel = await sut.GetTweetsByIdAsync(twitterIds);

                // Assert
                Assert.IsTrue(tweetStreamViewModel.Count == 1, "Expected 1 ViewModel");
                Assert.IsTrue(tweetStreamViewModel[0].Contents[0].TweetTime == now.ToString(CultureInfo.InvariantCulture));
                Assert.IsTrue(tweetStreamViewModel[0].Contents[1].TweetTime == twoDaysAgo.ToString(CultureInfo.InvariantCulture));
                Assert.IsTrue(tweetStreamViewModel[0].Contents[2].TweetTime == threeDaysAgo.ToString(CultureInfo.InvariantCulture));
            }
        }

    }
}
