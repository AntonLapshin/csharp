using System;
using System.ServiceModel;
using ChatService.Infrastructure.Abstract;
using ChatService.Infrastructure.Concrete;

namespace ChatWcfService
{
    public class ChatServiceHostFactory : ServiceHostFactory
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;

        public ChatServiceHostFactory()
        {
            var contextProvider = new ContextProvider();
            _messageRepository = new MessageRepository(contextProvider);
            _userRepository = new UserRepository(contextProvider);
        }

        public override ServiceHost CreateServiceHost(Type serviceType,
            Uri[] baseAddresses)
        {
            return new ChatServiceHost(_messageRepository, _userRepository, serviceType, baseAddresses);
        }
    }
}
