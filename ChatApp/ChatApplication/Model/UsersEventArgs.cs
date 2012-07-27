using System;
using System.Collections.Generic;
using ChatApplication.ChatServiceReference;

namespace ChatApplication.Model
{
    public class UsersEventArgs : EventArgs
    {
        public List<UserDTO> Users { get; set; } 
    }
}
