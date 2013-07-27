using System;
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
        protected static TwitterStreamHub SUT(Mock<ITwitterStreamService> mockTwitterStreamService = null)
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




    }
}
