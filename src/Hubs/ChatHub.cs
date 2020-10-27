using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ScalingSignalR.Hubs
{
    public class ChatHub : Hub
    {
        public async Task BroadcastMessage(string message)
        {
            await Clients.Others.SendAsync("MessageReceived", message);
        }
    }
}
