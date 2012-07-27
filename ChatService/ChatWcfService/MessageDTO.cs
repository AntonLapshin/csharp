using System;
using System.Runtime.Serialization;

namespace ChatWcfService
{
    [DataContract]
    public class MessageDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string MessageText { get; set; }
        [DataMember]
        public DateTime Timestamp { get; set; }
        [DataMember]
        public virtual UserDTO User { get; set; }
        [DataMember]
        public int UserNum { get; set; }
    }
}
