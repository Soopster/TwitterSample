﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using TwitterSample.Services;

namespace TwitterSample.Hubs
{
    public class TwitterStreamHub : Hub
    {
        private ITwitterStreamService _twitterStreamService;

        public TwitterStreamHub(ITwitterStreamService twitterStreamService)
        {
            if (twitterStreamService == null)
            {
                throw new ArgumentNullException("twitterStreamService");
            }

            _twitterStreamService = twitterStreamService;
        }

        public async Task<IEnumerable<TweetStreamViewModel>> GetTweets(string[] ids)
        {
            if (ids == null)
            {
                throw new ArgumentNullException("ids");
            }

            //Clients.All.streamTweets("Test1234");
            var twitterIds = new List<string>(ids);
            var tweetStream = await _twitterStreamService.GetTweetsByIdAsync(twitterIds);

            return tweetStream;
        }
    }
}