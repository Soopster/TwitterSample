using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TwitterSample.Models;

namespace TwitterSample.Services
{
    public class TwitterService : ITwitterService
    {  
        private readonly HttpClient _httpClient;
        private readonly ITwitterAuthService _twitterAuthService;
        private string _accessToken;

        public HttpClient HttpClient
        {
            get { return _httpClient; }
        }

        public TwitterService(ITwitterAuthService twitterAuthService, HttpClient httpClient = null)
        {
            if (twitterAuthService == null)
            {
                throw new ArgumentNullException("twitterAuthService");
            }

            _twitterAuthService = twitterAuthService;
            _httpClient = httpClient ?? new HttpClient();
        }

        public async Task<List<Tweet>> GetTimeLineByIdAsync(string twitterId, DateTime? tweetsSince = null)
        {
            var result = new List<Tweet>();
            _accessToken = await _twitterAuthService.GetAccessTokenAsync();

            try
            {
                if (!tweetsSince.HasValue)
                {
                    tweetsSince = DateTime.Now.AddDays(-14);
                }

                var response = await CallTwitterApi(twitterId, tweetsSince.Value);

                response.EnsureSuccessStatusCode();

                var jsonResult = await response.Content.ReadAsStringAsync();
                dynamic searchResult = System.Web.Helpers.Json.Decode(jsonResult);

                foreach (var status in searchResult.statuses)
                {
                    var createdAt = DateTime.ParseExact(status.created_at, @"ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture); 
                    var accountId = status.user.screen_name;
                    var content = status.text;

                    var tweet = new Tweet(createdAt, accountId, content);
                    result.Add(tweet);
                }

                return result;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<HttpResponseMessage> CallTwitterApi(string twitterId, DateTime tweetsSince)
        {
            // https://api.twitter.com/1.1/search/tweets.json?q=%3Dfrom%3APay_By_Phone%20since%3A2013-07-10
            //var requestUri =
                //new Uri(string.Format("https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name={0}", twitterId));
            var dateSinceString = tweetsSince.ToString("yyyy-MM-dd");

            var requestUri = new Uri(string.Format("https://api.twitter.com/1.1/search/tweets.json?q=%3Dfrom%3A{0}%20since%3A{1}&count=100", twitterId, dateSinceString));
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

            httpRequestMessage.Headers.Add("Authorization", "Bearer " + _accessToken);
            var response = await _httpClient.SendAsync(httpRequestMessage);

            return response;
        }
    }
}