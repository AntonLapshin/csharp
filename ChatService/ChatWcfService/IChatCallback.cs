using System.Collections.Generic;
using System.ServiceModel;

namespace ChatWcfService
{
    public interface IChatCallback
    {
        [OperationContract(IsOneWay = true)]
        void RefreshClients(List<UserDTO> users);

        [OperationContract(IsOneWay = true)]
        void Receive(MessageDTO msg);

        [OperationContract(IsOneWay = true)]
        void ReturnUser(UserDTO msg);
    }
}
