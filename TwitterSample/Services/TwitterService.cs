using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public async Task<List<Tweet>> GetTimeLineByIdAsync(string twitterId, DateTime? tweetsSince = null)
        {
            var result = new List<Tweet>();

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
            // get authentication token
            HttpMessageHandler handler = new HttpClientHandler();
            var httpClient = new HttpClient(handler);
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.twitter.com/oauth2/token ");
            var customerInfo = Convert.ToBase64String(new UTF8Encoding().GetBytes(this._consumerKey + ":" + this._consumerKeySecret));
            request.Headers.Add("Authorization", "Basic " + customerInfo);
            request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

            var oAuthResponse = await httpClient.SendAsync(request);

            var s = await oAuthResponse.Content.ReadAsStringAsync();
            var returnJson = JValue.Parse(s);
            var accessToken = returnJson["access_token"].ToString();

            // https://api.twitter.com/1.1/search/tweets.json?q=%3Dfrom%3APay_By_Phone%20since%3A2013-07-10
            //var requestUri =
                //new Uri(string.Format("https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name={0}", twitterId));
            var dateSinceString = tweetsSince.ToString("yyyy-MM-dd");

            var requestUri = new Uri(string.Format("https://api.twitter.com/1.1/search/tweets.json?q=%3Dfrom%3A{0}%20since%3A{1}", twitterId, dateSinceString));
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
            httpRequestMessage.Headers.Add("Authorization", "Bearer " + accessToken);
            var response = await _httpClient.SendAsync(httpRequestMessage);

            //var response = await _httpClient.GetAsync(requestUri);
            return response;
        }
    }
}