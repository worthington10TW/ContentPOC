using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ContentPOC.NewsIngestor
{
    public class NotificationHub : Hub
    {
        public async Task NewsUpdated()
        {
            await Clients.All.SendAsync("NewsUpdated");
        }
    }
}
