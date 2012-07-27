using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace ChatWcfService
{
    [ServiceContract(CallbackContract = typeof(IChatCallback),
                                 SessionMode = SessionMode.Required)]
    public interface IChat
    {
        [ServiceKnownType(typeof(ConnectedStatuses))]
        [OperationContract(IsInitiating = true)]
        ConnectedStatuses Connect(UserDTO client);

        [OperationContract(IsOneWay = true)]
        void Say(MessageDTO msg);

        [OperationContract]
        List<MessageDTO> GetLogByUser(UserDTO user);

        [OperationContract]
        List<MessageDTO> GetLogByRangeDate(DateTime from, DateTime to);

        [OperationContract]
        List<MessageDTO> GetLog();

        [OperationContract]
        List<UserDTO> GetAllUsers();

        [OperationContract(IsOneWay = true, IsTerminating = true)]
        void Disconnect(UserDTO client);
    }
}
