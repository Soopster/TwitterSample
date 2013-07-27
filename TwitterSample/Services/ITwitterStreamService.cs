using System.Collections.Generic;

namespace TwitterSample.Services
{
    public interface ITwitterStreamService
    {
        IList<TweetStream> GetTweetsById(List<string> twitterIds);
    }

    public class TweetStream
    {
    }
}