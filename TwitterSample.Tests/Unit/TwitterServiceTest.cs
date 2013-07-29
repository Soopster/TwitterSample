using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Fakes;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.QualityTools.Testing.Fakes;
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

        protected static TwitterService GetTestTwitterService(HttpClient httpClient = null)
        {
            if (httpClient == null)
            {
                httpClient = new HttpClient();
            }

            var sut = new TwitterService("accessToken", "accessTokenSecret", "consumerKey", "consumerKeySecret", httpClient);
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
            public async Task WillReturnTweetsBasedOnAId()
            {
                // Arrange
                using (ShimsContext.Create())
                {
                    var shimHttpClient = new ShimHttpClient
                    {
                        SendAsyncHttpRequestMessage = (x) =>
                        {
                            var response = @"[{
                                ""created_at"": ""Tue Jun 18 15:32:21 +0000 2013"",
                                ""id"": 347013925633679360,
                                ""id_str"": ""347013925633679361"",
                                ""text"": ""Test tweet"",
                                ""user"": {
                                  ""id"": 12345,
                                  ""id_str"": ""12345"",
                                  ""name"": ""Test Name"",
                                  ""screen_name"": ""Tester"",
                                  ""utc_offset"": 28800,
                                  ""time_zone"": ""Perth"",
                                  ""profile_background_color"": ""131516"",
                                  ""profile_background_image_url"": ""http://a0.twimg.com/images/themes/theme14/bg.gif"",
                                  ""profile_image_url"": ""http://a0.twimg.com/profile_images/1.jpg""
                                },
                                ""favorited"": false,
                                ""retweeted"": false,
                                ""lang"": ""en""
                              }]";
                            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                            {
                                Content = new StringContent(response)
                            };

                            return Task.FromResult(httpResponseMessage);
                        }
                    };

                    var sut = GetTestTwitterService(shimHttpClient.Instance);
                    
                    // Act
                    var result = await sut.GetTimeLineByIdAsync("@test");

                    // Assert
                    Assert.IsNotNull(result);
                    Assert.IsTrue(result.Count == 1);
                    Assert.IsTrue(result[0].AccountId == "Tester");
                    Assert.IsTrue(result[0].Content == "Test tweet");
                } 
            }
        }
    }
}
