using System;
using System.Globalization;

namespace TwitterSample.Services
{
    public class TweetContent
    {
        public string TweetTime { get; private set; }
        public string Tweet { get; private set; }

        public TweetContent(DateTime tweetTime, string tweet)
        {
            TweetTime = tweetTime.ToString(CultureInfo.InvariantCulture);
            Tweet = tweet;
        }
    }
}