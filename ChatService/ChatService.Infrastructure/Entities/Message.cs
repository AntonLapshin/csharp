using System;

namespace ChatService.Infrastructure.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int UserNum { get; set; }
        public string MessageText { get; set; }
        public DateTime Timestamp { get; set; }
        public virtual User User { get; set; }
    }
}
