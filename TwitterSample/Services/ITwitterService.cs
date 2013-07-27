using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace TwitterSample.Services
{
    public interface ITwitterService
    {
        Task<List<Tweet>> GetTimeLineByIdAsync(string twitterId);
    }

    public class TwitterService : ITwitterService
    {
        public Task<List<Tweet>> GetTimeLineByIdAsync(string twitterId)
        {
            throw new System.NotImplementedException();
        }
    }

    public class Tweet
    {
        private readonly DateTime _time;

        public DateTime Time
        {
            get { return _time; }
        }

        public Tweet(DateTime time)
        {
            _time = time;
        }
    }
}