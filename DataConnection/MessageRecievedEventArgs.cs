namespace ChatPage_New.DataConnection{
     public class MessageRecievedEventArgs : EventArgs
    {
        public string Username { get; set; }
        public string Message { get; set; }
        public MessageRecievedEventArgs(string username, string message)
        {
            Username = username;
            Message = message;
        }
    }
}