using System;
using System.Globalization;

namespace TwitterSample.Services
{
    public class TweetContent
    {
        public string TweetTime { get; private set; }

        public TweetContent(DateTime tweetTime)
        {
            TweetTime = tweetTime.ToString(CultureInfo.InvariantCulture);
        }
    }
}