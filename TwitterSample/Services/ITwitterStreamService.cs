using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterSample.Models;

namespace TwitterSample.Services
{
    public interface ITwitterStreamService
    {
        Task<IList<TweetStreamViewModel>> GetTweetsByIdAsync(List<string> twitterIds);
    }
}