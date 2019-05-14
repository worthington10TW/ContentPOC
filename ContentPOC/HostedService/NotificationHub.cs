using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ContentPOC.NewsIngestor
{
    public class NotificationHub : Hub
    {
        public NotificationHub()
        {
        }

        public async Task SendMyEvent()
        {
            await Clients.All.SendAsync("NewsUpdated");
        }
    }
}
