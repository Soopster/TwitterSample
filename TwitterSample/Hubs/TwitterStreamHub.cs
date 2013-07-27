using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace TwitterSample.Hubs
{
    
    public class TwitterStreamHub : Hub
    {
        public void GetTweets()
        {
            Clients.All.streamTweets("Test1234");
        }
    }
}