using System.Collections.Generic;

namespace TwitterSample.Services
{
    public class TweetStreamViewModel
    {
        public List<TweetContent> Contents { get; set; }

        public TweetStreamViewModel()
        {
            this.Contents = new List<TweetContent>();

        }
    }
}