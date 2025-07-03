using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.SignalR
{
    public class ChatHub:Hub
    {
        public async ValueTask SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

    }
}
