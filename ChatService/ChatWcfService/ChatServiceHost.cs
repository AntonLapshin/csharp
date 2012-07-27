using System;
using System.ServiceModel;
using ChatService.Infrastructure.Abstract;

namespace ChatWcfService
{
    public class ChatServiceHost : ServiceHost
    {
        public ChatServiceHost(IMessageRepository messageRepository, IUserRepository userRepository, Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            if (messageRepository==null)
            {
                throw new ArgumentNullException("messageRepository");
            }
            if (userRepository == null)
            {
                throw new ArgumentNullException("userRepository");
            }

            foreach (var cd in ImplementedContracts.Values)
            {
                cd.Behaviors.Add(new ChatInstanceProvider(messageRepository,userRepository));
            }
        }
    }
}