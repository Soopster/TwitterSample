using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TwitterSample.Services
{
    public class TwitterAuthService : ITwitterAuthService
    {
        private readonly string _accessToken;
        private readonly string _accessTokenSecret;
        private readonly string _consumerKey;
        private readonly string _consumerKeySecret;
        private readonly HttpClient _httpClient;

        public TwitterAuthService()
            : this(Properties.Settings.Default.accessToken, Properties.Settings.Default.accessTokenSecret, Properties.Settings.Default.consumerKey, Properties.Settings.Default.consumerKeySecret, null)
        {
            
        }

        public TwitterAuthService(string accessToken, string accessTokenSecret, string consumerKey, string consumerKeySecret, HttpClient httpClient = null)
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
            _httpClient = httpClient ?? new HttpClient(new HttpClientHandler());
        }

        public async Task<string> GetAccessTokenAsync()
        {
            // get authentication token

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.twitter.com/oauth2/token ");
            var authInfo = Convert.ToBase64String(new UTF8Encoding().GetBytes(this._consumerKey + ":" + this._consumerKeySecret));
            
            request.Headers.Add("Authorization", "Basic " + authInfo);
            request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

            var oAuthResponse = await _httpClient.SendAsync(request);
            var jsonResult = await oAuthResponse.Content.ReadAsStringAsync();
            
            dynamic returnJson = System.Web.Helpers.Json.Decode(jsonResult);
            var accessToken = returnJson.access_token;

            return accessToken;
        }
    }
}