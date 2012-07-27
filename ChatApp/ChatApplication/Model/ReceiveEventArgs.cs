using System;
using ChatApplication.ChatServiceReference;

namespace ChatApplication.Model
{
    public class ReceiveEventArgs : EventArgs
    {
        public MessageDTO Message { get; set; }
    }
}
