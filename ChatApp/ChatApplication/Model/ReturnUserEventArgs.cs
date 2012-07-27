using System;
using ChatApplication.ChatServiceReference;

namespace ChatApplication.Model
{
    public class ReturnUserEventArgs :EventArgs
    {
        public UserDTO User { get; set; }
    }
}
