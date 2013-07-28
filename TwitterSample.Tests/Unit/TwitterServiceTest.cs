using System;
using System.Net.Http;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwitterSample.Services;
using TwitterSample.Tests.Utils;

namespace TwitterSample.Tests.Unit
{
    [TestClass]
    public class TwitterServiceTest
    {
        protected static TwitterService GetTestTwitterService(Mock<HttpClient> mockHttpClient = null)
        {
            if (mockHttpClient == null)
            {
                mockHttpClient = new Mock<HttpClient>();
            }
            
            var sut = new TwitterService("accessToken", "accessTokenSecret", "consumerKey", "consumerKeySecret", mockHttpClient.Object);
            return sut;
        }

        [TestClass]
        public class TheConstructor
        {
            [TestMethod]
            public void GivenNullParameters_ItShouldThrow()
            {
                ContractAssert.ThrowsArgNull(() => new TwitterService(null, "accessTokenSecret", "consumerKey", "consumerKeySecret"), "accessToken");
                ContractAssert.ThrowsArgNull(() => new TwitterService("accessToken", null, "consumerKey", "consumerKeySecret"), "accessTokenSecret");
                ContractAssert.ThrowsArgNull(() => new TwitterService("accessToken", "accessTokenSecret", null, "consumerKeySecret"), "consumerKey");
                ContractAssert.ThrowsArgNull(() => new TwitterService("accessToken", "accessTokenSecret", "consumerKey", null), "consumerKeySecret");
            }
        }

        [TestClass]
        public class TheGetTimeLineByIdAsyncMethod
        {
            [TestMethod]
            public void WillReturnTweetsBasedOnAId()
            {
                // Arrange
                var mockHttpClient = new Mock<HttpClient>();

                var sut = GetTestTwitterService(mockHttpClient);
                // Act

                // Assert
            }   
        }
    }
}
