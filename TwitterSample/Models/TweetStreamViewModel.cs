using System;
using System.Collections.Generic;
using System.Linq;

namespace TwitterSample.Models
{
    public class TweetStreamViewModel
    {
        public List<TweetContent> Contents { get; set; }
        public string TwitterAccount { get; set; }

        public int NumberOfTweets
        {
            get
            {
                return Contents.Count;
            }
        }

        public int NumberOfMentionsOfOthers
        {
            get
            {
                return GetNumberOfMetionsOfOthers();
            }
        }

        public TweetStreamViewModel()
        {
            this.Contents = new List<TweetContent>();
        }

        private int GetNumberOfMetionsOfOthers()
        {
            var thisAccount = TwitterAccount;

            if (!this.TwitterAccount.StartsWith("@"))
            {
                thisAccount = string.Format("@{0}", thisAccount);
            }
          
            // Find all Tweets starting with "@" indicating a mention.
            // Don't include any mentions to yourself
            var mentions = from content in this.Contents
                from mention in content.Tweet.Split(' ')
                where mention.StartsWith("@")
                && !mention.Equals(thisAccount, StringComparison.OrdinalIgnoreCase)
                select mention;

            var result = mentions.Count();

            return result;
        }

    }
}