using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using TwitterSample.Hubs;
using TwitterSample.Models;
using TwitterSample.Services.Mapping;

namespace TwitterSample.Services
{
    public class TwitterStreamService : ITwitterStreamService
    {
        private readonly ITwitterService _twitterService;
        private readonly IHubConnectionContext _clients;
        private readonly ConcurrentDictionary<string, List<Tweet>> _tweets;
        private Timer _timer;
        private volatile bool _updatingTweets = false;
        private readonly TimeSpan _updateInterval = TimeSpan.FromSeconds(30);
        private readonly object _updateTweetsLock = new object();

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

            _timer = new Timer(UpdateTweets, null, _updateInterval, _updateInterval);
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

        private void UpdateTweets(object state)
        {
            lock (_updateTweetsLock)
            {
                if (!_updatingTweets)
                {
                    _updatingTweets = true;

                    var twitterIds = _tweets.Keys.ToList();
                    twitterIds.Reverse();

                    _tweets.Clear();

                    this.GetTweetsByIdAsync(twitterIds).ContinueWith(x =>
                    {
                        IEnumerable<TweetStreamViewModel> tweetStreamViewModels = x.Result;
                        var clients = GlobalHost.ConnectionManager.GetHubContext<TwitterStreamHub>().Clients;

                        clients.All.updateStream(tweetStreamViewModels);
                    });
                    
                    _updatingTweets = false;
                }
            }
        }
    }
}