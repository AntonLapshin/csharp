using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using AutoMapper;
using ChatService.Infrastructure.Abstract;
using ChatService.Infrastructure.Concrete;
using ChatService.Infrastructure.Entities;

namespace ChatWcfService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        UseSynchronizationContext = false)]
    public class ChatService : IChat
    {
        private readonly Dictionary<UserDTO, IChatCallback> _users =
            new Dictionary<UserDTO, IChatCallback>();

        private readonly object _syncObj = new object();
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository; 

        public ChatService(IUserRepository userRepository, IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            CreateMappings();
        }

        public ChatService()
        {
            _userRepository = new UserRepository(new ContextProvider());
            _messageRepository = new MessageRepository(new ContextProvider());
            CreateMappings();
        }

        private static void CreateMappings()
        {
            Mapper.CreateMap<Message, MessageDTO>();
            Mapper.CreateMap<User, UserDTO>();
            Mapper.CreateMap<UserDTO, User>();
            Mapper.CreateMap<MessageDTO, Message>();
        }

        public IChatCallback CurrentCallback
        {
            get
            {
                return OperationContext.Current.
                    GetCallbackChannel<IChatCallback>();
            }
        }

        public ConnectedStatuses Connect(UserDTO user)
        {
            if (!_users.ContainsValue(CurrentCallback) &&
                !SearchClientsByName(user.Name)) //exists user in chatroom
            {
                lock (_syncObj)
                {
                    if (ContainsUser(user)) //exists user in db
                    {
                        if(!ValidateUserPassword(user))
                        {
                            return ConnectedStatuses.PasswordNotValid;
                        }
                    }
                    else
                    {
                        AddUserToDataBase(user);
                    }

                    RegisterUser(user); //register user in chatroom

                    foreach (var key in _users.Keys)
                    {
                        var callback = _users[key];
                        try
                        {
                            UpdateClients(callback);
                        }                        
                        catch
                        {
                            _users.Remove(key);
                            return ConnectedStatuses.Error;
                        }
                    }
                }
                return ConnectedStatuses.Connected;
            }
            return ConnectedStatuses.AlreadyConnected;
        }

        private void RegisterUser(UserDTO user)
        {
            var newUser = GetUserByName(user.Name);
            _users.Add(newUser, CurrentCallback);
            CurrentCallback.ReturnUser(newUser);
        }

        private UserDTO GetUserByName(string name)
        {
            return Mapper.Map<User, UserDTO>(_userRepository.GetUserByName(name));
        }

        private void AddUserToDataBase(UserDTO user)
        {
            _userRepository.Add(Mapper.Map<UserDTO, User>(user));
        }

        private bool ValidateUserPassword(UserDTO user)
        {
            var dbUser = _userRepository.GetUserByName(user.Name);

            dbUser.Password = user.Password;

            return dbUser.IsPasswordValid();
        }

        private bool ContainsUser(UserDTO user)
        {
            return _userRepository.Contains(user.Name);
        }

        private bool SearchClientsByName(string name)
        {
            return _users.Keys.Any(c => c.Name == name);
        }

        public void Say(MessageDTO msg)
        {
            var newMsg = Mapper.Map<MessageDTO, Message>(msg);
            BindingRealUser(newMsg); //ugly code, add binding user from database to sended message
            lock (_syncObj)
            {
                _messageRepository.Add(newMsg);
                foreach (var callback in _users.Values)
                {
                    callback.Receive(msg);
                }
            }
        }

        private void BindingRealUser(Message newMsg)
        {
            newMsg.User = _userRepository.GetUserByName(newMsg.User.Name);
            newMsg.UserNum = newMsg.User.Id;
        }

        public List<UserDTO> GetAllUsers()
        {
            return Mapper.Map<List<User>, List<UserDTO>>(_userRepository.GetAll().ToList());
        }

        public void Disconnect(UserDTO user)
        {
            foreach (var c in _users.Keys)
            {
                if (user.Name != c.Name) continue;

                lock (_syncObj)
                {
                    _users.Remove(c);
                    NotifyClientsForUserDisconnect();
                }
                return;
            }
        }

        private void NotifyClientsForUserDisconnect()
        {
            foreach (var callback in _users.Values)
            {
                UpdateClients(callback);
            }
        }

        private void UpdateClients(IChatCallback callback)
        {
            callback.RefreshClients(_users.Keys.ToList());
        }

        public List<MessageDTO> GetLogByUser(UserDTO user)
        {
            var byUser = Mapper.Map<UserDTO, User>(user);
            var messages = _messageRepository.GetMessagesByUser(byUser).ToList();
            return
                Mapper.Map<List<Message>, List<MessageDTO>>(messages);
        }

        public List<MessageDTO> GetLogByRangeDate(DateTime from, DateTime to)
        {
            var messages = _messageRepository.GetMessagesByRangeDate(from, to).ToList();
            return
                Mapper.Map<List<Message>, List<MessageDTO>>(messages);
        }

        public List<MessageDTO> GetLog()
        {
            var messages = _messageRepository.GetAll().ToList();
            return Mapper.Map<List<Message>, List<MessageDTO>>(messages);
        }
        
    }
}






