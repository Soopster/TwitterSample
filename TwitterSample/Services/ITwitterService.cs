using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using TwitterSample.Models;

namespace TwitterSample.Services
{
    public interface ITwitterService
    {
        Task<List<Tweet>> GetTimeLineByIdAsync(string twitterId, DateTime? tweetsSince = null);
    }
}