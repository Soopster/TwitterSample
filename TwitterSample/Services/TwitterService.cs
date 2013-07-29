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
        private readonly string _accessToken;
        private readonly string _accessTokenSecret;
        private readonly string _consumerKey;
        private readonly string _consumerKeySecret;
        private readonly HttpClient _httpClient;
        public HttpClient HttpClient
        {
            get { return _httpClient; }
        }

        public TwitterService() : this(Properties.Settings.Default.accessToken, Properties.Settings.Default.accessTokenSecret, Properties.Settings.Default.consumerKey, Properties.Settings.Default.consumerKeySecret, null)
        {
            
        }
        public TwitterService(string accessToken, string accessTokenSecret, string consumerKey, string consumerKeySecret, HttpClient httpClient = null)
        {
            if (accessToken == null)
            {
                throw new ArgumentNullException("accessToken");
            }
            if (accessTokenSecret == null)
            {
                throw new ArgumentNullException("accessTokenSecret");
            }
            if (consumerKey == null)
            {
                throw new ArgumentNullException("consumerKey");
            }
            if (consumerKeySecret == null)
            {
                throw new ArgumentNullException("consumerKeySecret");
            }
            _accessToken = accessToken;
            _accessTokenSecret = accessTokenSecret;
            _consumerKey = consumerKey;
            _consumerKeySecret = consumerKeySecret;

            _httpClient = httpClient ?? new HttpClient();
        }

        public async Task<List<Tweet>> GetTimeLineByIdAsync(string twitterId)
        {
            var result = new List<Tweet>();

            try
            {
                var requestUri = new Uri("http://www.google.com");
                var response = await _httpClient.GetAsync(requestUri);

                response.EnsureSuccessStatusCode();

                var jsonResult = await response.Content.ReadAsStringAsync();
                dynamic timeline = System.Web.Helpers.Json.Decode(jsonResult);

                foreach (var timeLineTweet in timeline)
                {
                    var createdAt = DateTime.ParseExact(timeLineTweet.created_at, @"ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture); 
                    var accountId = timeLineTweet.user.screen_name;
                    var content = timeLineTweet.text;

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
    }
}