using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using TwitterSample.Models;
using TwitterSample.Services.Mapping;

namespace TwitterSample.Services
{
    public class TwitterStreamService : ITwitterStreamService
    {
        private readonly ITwitterService _twitterService;
        private readonly IHubConnectionContext _clients;
        private ConcurrentDictionary<string, List<Tweet>> _tweets;

        public TwitterStreamService(ITwitterService twitterService, IHubConnectionContext clients)
        {
            // Code contracts (eg: Contract.Requires<>) aren't currently supported in VS 2013, so using simple Null checks.

            if (twitterService == null)
            {
                throw new ArgumentNullException("twitterService");
            }
            if (clients == null)
            {
                throw new ArgumentNullException("clients");
            }
            
            _clients = clients;            
            _twitterService = twitterService;
            _tweets = new ConcurrentDictionary<string, List<Tweet>>();
        }

        public async Task<IList<TweetStreamViewModel>> GetTweetsByIdAsync(List<string> twitterIds)
        {
            var tweetStreamViewModels = new List<TweetStreamViewModel>();

            foreach (var twitterId in twitterIds)
            {
                TweetStreamViewModel tweetStreamViewModel = null;
                List<Tweet> tweets = null;
                if (!this._tweets.ContainsKey(twitterId))
                {
                    tweets = await _twitterService.GetTimeLineByIdAsync(twitterId);
                    _tweets.TryAdd(twitterId, tweets);
                    tweetStreamViewModel = tweets.MapToTweetStreamViewModel(); 
                    tweetStreamViewModels.Add(tweetStreamViewModel);
                }
                else
                {   
                    this._tweets.TryGetValue(twitterId, out tweets);
                    tweetStreamViewModel = tweets.MapToTweetStreamViewModel();
                    tweetStreamViewModels.Add(tweetStreamViewModel);
                }
            }

            return tweetStreamViewModels;
        }
    }
}