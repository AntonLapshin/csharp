using System.Runtime.Serialization;

namespace ChatWcfService
{
    [DataContract]
    public enum ConnectedStatuses
    {
        [EnumMember]
        None=0,
        [EnumMember]
        AlreadyConnected=1,
        [EnumMember]
        PasswordNotValid=2,
        [EnumMember]
        Connected=3,
        [EnumMember]
        Error=4
    }
}
