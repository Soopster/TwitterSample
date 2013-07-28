using System;
using System.Collections.Generic;
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

        public Task<List<Tweet>> GetTimeLineByIdAsync(string twitterId)
        {
            //dynamic timeline = System.Web.Helpers.Json.Decode(response);
            throw new System.NotImplementedException();
        }
    }
}