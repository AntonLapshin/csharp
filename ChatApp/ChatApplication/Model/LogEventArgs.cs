using System;
using System.Collections.Generic;
using ChatApplication.ChatServiceReference;

namespace ChatApplication.Model
{
    public class LogEventArgs:EventArgs
    {
        public List<MessageDTO> Log;
    }
}
