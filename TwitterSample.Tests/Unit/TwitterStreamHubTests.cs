using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwitterSample.Hubs;
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
            public void WillReturnAListOfTweetsWhenCalledWithIds()
            {
                // Arranage
                var mockTwitterStreamService = new Mock<ITwitterStreamService>();
                var twitterIds = new List<string> { "test1", "test2" };
                var tweetStreams = new List<TweetStream>(){new TweetStream(), new TweetStream()};

                mockTwitterStreamService.Setup(x => x.GetTweetsById(twitterIds)).Returns(tweetStreams);

                var sut = GetTestTwitterStreamHub(mockTwitterStreamService: mockTwitterStreamService);

                // Act
                var tweetStream = sut.GetTweets(twitterIds.ToArray());

                // Assert
                mockTwitterStreamService.Verify(x => x.GetTweetsById(twitterIds));

                Assert.IsNotNull(tweetStream);
                Assert.IsTrue(tweetStream.Count() == 2);
            }

            [TestMethod]
            public void WillThrowWhenCalledWithNoIDs()
            {
                // Arranage
                var sut = GetTestTwitterStreamHub();

                // Assert
                ContractAssert.ThrowsArgNull(() => sut.GetTweets(null), "ids");
            }
        }

    }
}
