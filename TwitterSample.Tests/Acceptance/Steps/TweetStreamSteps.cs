using System;
using System.Data;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using TwitterSample.Services;

namespace TwitterSample.Tests
{
    [Binding]
    public class TweetStreamSteps
    {
        private int _tweets;

        [Given]
        public void Given_I_have_an_API_key_for_twitter()
        {
            var accessToken = "2148771-lAyLM5At75ZLhDLNCQ8ijH0YrrvX4ntrSnuv1ySoPg";
            var accessTokenSecret = "yBQPXj9bH2NpJ6Y0ns2cHd5l8lL5G5C00JUTm5A9KcU";
            var consumerKey = "O44DgOFQ7Ps8jyQMU3SWsA";
            var consumerKeySecret = "3PwT7czs7xnxjvgH3laPFz1kMlWCxfQWkwr10YQ";

            var twitterAuthService = new TwitterAuthService(accessToken, accessTokenSecret, consumerKey,
                consumerKeySecret);

            var twitterService = new TwitterService(twitterAuthService, null);
            ScenarioContext.Current.Set(twitterService);
        }

        private AutoResetEvent _autoReset = new AutoResetEvent(false);

        [When]
        public void When_I_call_the_service_with_the_following_Twitter_Ids(Table table)
        {
            var twitterService = ScenarioContext.Current.Get<TwitterService>();

            foreach (var tableRow in table.Rows)
            {
                var id = tableRow["Id"];
                twitterService.GetTimeLineByIdAsync(id).ContinueWith(x =>
                {
                    _tweets = x.Result.Count;
                    _autoReset.Set();
                });
            }
            
        }
        
        [Then]
        public void Then_tweets_should_be_returned()
        {
            _autoReset.WaitOne();
            Assert.IsTrue(_tweets > 0);
        }
    }
}
