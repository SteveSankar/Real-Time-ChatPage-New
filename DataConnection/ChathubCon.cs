
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace ChatPage_New.DataConnection
{
    public class ChathubCon : IAsyncDisposable
    {
        private readonly NavigationManager _navigationManager;
        private readonly string _username;
        private HubConnection _hubConnection;
        private bool _Started = false;
        public ChathubCon(string username, NavigationManager navigationManager)
        {
            _username = username;
            _navigationManager = navigationManager;
            //MessageRecieved += (sender, args) => { };
        }

        public async Task StartChatMethod()
        {
            if (!_Started)
            {
                _hubConnection = new HubConnectionBuilder()
                                .WithUrl(_navigationManager.ToAbsoluteUri("/chathub"))
                                 .WithAutomaticReconnect()
                                 .Build();
                // how does call this 
                _hubConnection.On<string, string>("RecieveMessageMethod", (username, message) =>
                {
                    MessageRecieved?.Invoke(this, new MessageRecievedEventArgs(username, message));
                });

                await _hubConnection.StartAsync();
                _Started = true;
                await _hubConnection.SendAsync("RegisterInHub", _username);
            }
        }
        public delegate void MessageRecievedEventHandler(object sender, MessageRecievedEventArgs e);
        public event MessageRecievedEventHandler MessageRecieved;// = (sender, args) => { };

        public async Task NewMessageTaker(string messege)
        {
            if (!_Started)
            {
                throw new InvalidOperationException("Client not Started");
            }
            await _hubConnection.SendAsync("SendMessage", _username, messege);
        }
        public async ValueTask DisposeAsync()
        {
            await StopAsync();
        }

        public async Task StopAsync()
        {
            if (_Started)
            {
                await _hubConnection.StopAsync();
                await _hubConnection.DisposeAsync();
                _hubConnection = null;
                _Started = false;
            }
        }
    }


   
}
