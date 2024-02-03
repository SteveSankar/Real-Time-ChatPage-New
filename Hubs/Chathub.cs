using Microsoft.AspNetCore.SignalR;

namespace ChatPage_New.Hubs
{
    public class ChatHub : Hub
    {
        public static readonly Dictionary<string, string> userLookUp = new Dictionary<string, string>();

        public async Task SendMessage(string username, string messege)
        {
            await Clients.All.SendAsync("RecieveMessageMethod", username, messege);
        }
        public async Task RegisterInHub(string username)
        {
            var currentId = Context.ConnectionId;
            if (!userLookUp.ContainsKey(currentId))
            {
                userLookUp.Add(currentId, username);
                await Clients.AllExcept(currentId).SendAsync("RecieveMessageMethod","", $"{username} Joined the Chat");
            }
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            //Console.WriteLine($"DisConnected.{exception.Message} {Context.ConnectionId}");
            var currentid = Context.ConnectionId;
            if (!userLookUp.TryGetValue(currentid, out string? username))
            {
                username = "[Unknown]";
            }
            userLookUp.Remove(currentid);
            Clients.AllExcept(currentid).SendAsync("RecieveMessageMethod", username, $"{username} has Left the Chat");
            return base.OnDisconnectedAsync(exception);
        }

    }
}