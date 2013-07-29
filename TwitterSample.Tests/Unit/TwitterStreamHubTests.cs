using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Language;
using Moq.Language.Flow;
using TwitterSample.Hubs;
using TwitterSample.Models;
using TwitterSample.Services;
using TwitterSample.Tests.Utils;

namespace TwitterSample.Tests
{
    [TestClass]
    public class TwitterStreamHubTests
    {
        protected static TwitterStreamHub GetTestTwitterStreamHub(Mock<ITwitterStreamService> mockTwitterStreamService = null)
        {
            if (mockTwitterStreamService == null)
            {
                mockTwitterStreamService = new Mock<ITwitterStreamService>();
            }

            var sut = new TwitterStreamHub(mockTwitterStreamService.Object);
            return sut;
        }

        [TestClass]
        public class TheConstructor
        {
            [TestMethod]
            public void GivenANullTwitterStreamService_ItShouldThrow()
            {
                ContractAssert.ThrowsArgNull(() => new TwitterStreamHub(null), "twitterStreamService");
            }
        }

        [TestClass]
        public class TheGetTweetsMethod
        {
            [TestMethod]
            public async Task WillReturnAListOfTweetsWhenCalledWithIds()
            {
                // Arranage
                var mockTwitterStreamService = new Mock<ITwitterStreamService>();
                var twitterIds = new List<string> { "@test1", "@test2" };
                var tweetStreamViewModels = new List<TweetStreamViewModel>() { new TweetStreamViewModel(), new TweetStreamViewModel() };

                mockTwitterStreamService.Setup(x => x.GetTweetsByIdAsync(twitterIds)).ReturnsAsync(tweetStreamViewModels);

                var sut = GetTestTwitterStreamHub(mockTwitterStreamService: mockTwitterStreamService);

                // Act
                var tweetStream = await sut.GetTweets(twitterIds.ToArray());

                // Assert
                mockTwitterStreamService.Verify(x => x.GetTweetsByIdAsync(twitterIds));

                Assert.IsNotNull(tweetStream);
                Assert.IsTrue(tweetStream.Count() == 2);
            }

            [TestMethod]
            public async Task WillThrowWhenCalledWithNoIDs()
            {
                // Arranage
                var sut = GetTestTwitterStreamHub();
               
                // Assert
                try
                {
                    await sut.GetTweets(null);
                }
                catch (ArgumentNullException ex)
                {
                    Assert.IsTrue(ex.ParamName == "ids");
                }
            }
        }

    }
}
