using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterSample.Services.Mapping
{
    public static class ViewModelMappings
    {
        public static TweetStreamViewModel MapToTweetStreamViewModel(this List<Tweet> tweets)
        {
            var tweetStreamViewModel = new TweetStreamViewModel();
            foreach (var tweet in tweets.OrderByDescending(x => x.Time))
            {
                var tweetContent = new TweetContent(tweet.Time);

                tweetStreamViewModel.Contents.Add(tweetContent);
            }

            return tweetStreamViewModel;
        }
    }
}