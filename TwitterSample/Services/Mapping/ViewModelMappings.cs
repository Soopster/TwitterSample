using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TwitterSample.Models;

namespace TwitterSample.Services.Mapping
{
    public static class ViewModelMappings
    {
        public static TweetStreamViewModel MapToTweetStreamViewModel(this List<Tweet> tweets)
        {
            var tweetStreamViewModel = new TweetStreamViewModel();
            string twitterAccountId = string.Empty;
            foreach (var tweet in tweets.OrderByDescending(x => x.Time))
            {
                var tweetContent = new TweetContent(tweet.Time, tweet.Content);
                twitterAccountId = tweet.AccountId;
                tweetStreamViewModel.Contents.Add(tweetContent);
            }
            tweetStreamViewModel.TwitterAccount = twitterAccountId;
            return tweetStreamViewModel;
        }
    }
}