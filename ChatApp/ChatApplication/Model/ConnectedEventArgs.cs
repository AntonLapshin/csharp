using System;

namespace ChatApplication.Model
{
    public class ConnectedEventArgs:EventArgs
    {
        public bool Result { get; set; }
        public string Message { get; set; }
    }
}
