using System;

namespace TwitterSample.Models
{
    public class Tweet
    {
        private readonly DateTime _time;
        private string _accountId;
        private string _content;

        public DateTime Time
        {
            get { return _time; }
        }

        public string AccountId
        {
            get { return _accountId; }
        }

        public string Content
        {
            get { return _content; }
        }

        public Tweet(DateTime dateTime, string accountId, string content)
        {
            _time = dateTime;
            _accountId = accountId;
            _content = content;
        }
    }
}