using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterSample.Models;

namespace TwitterSample.Services
{
    public class TwitterService : ITwitterService
    {
        public Task<List<Tweet>> GetTimeLineByIdAsync(string twitterId)
        {
            throw new System.NotImplementedException();
        }
    }
}