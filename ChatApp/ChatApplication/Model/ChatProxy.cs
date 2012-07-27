using System;
using System.Collections.Generic;
using System.ServiceModel;
using ChatApplication.ChatServiceReference;

namespace ChatApplication.Model
{
    public class ChatProxy : IChatCallback
    {
        private ChatClient _chatClient;
        public UserDTO CurrentUser { get; set; }
        public event EventHandler<ConnectedEventArgs> Connected;
        public event EventHandler<UsersEventArgs> RefreshUsers;
        public event EventHandler<ReceiveEventArgs> ReceiveMessage;
        public event EventHandler<ExceptionEventArgs> Exception; //event for handle exception
        public event EventHandler<LogEventArgs> GetLog;
        public event EventHandler<UsersEventArgs> GetUsers;
        public event EventHandler<ReturnUserEventArgs> ReturnedUser;

        public void Connect(string host, string port, UserDTO user)
        {
            try
            {
                CurrentUser = user;
                _chatClient = new ChatClient(new InstanceContext(this));

                var servicePath = _chatClient.Endpoint.ListenUri.AbsolutePath;
                _chatClient.Endpoint.Address = new EndpointAddress("net.tcp://" + host + ":" + port + servicePath);

                _chatClient.Open();

                RegisterServiceEvents();

                _chatClient.ConnectAsync(CurrentUser);
            }
            catch (Exception ex)
            {
                Exception(this, new ExceptionEventArgs { Message = ex.Message });
            }
        }

        private void RegisterServiceEvents()
        {
            _chatClient.ConnectCompleted += ChatClientConnectCompleted;
            _chatClient.GetLogByRangeDateCompleted += ChatClientGetLogByRangeDateCompleted;
            _chatClient.GetLogCompleted += ChatClientGetLogCompleted;
            _chatClient.GetLogByUserCompleted += ChatClientGetLogByUserCompleted;
            _chatClient.GetAllUsersCompleted += GetAllUsersCompleted;
        }

        void ChatClientConnectCompleted(object sender, ConnectCompletedEventArgs e)
        {
            try
            {
                switch (e.Result)
                {
                    case ConnectedStatuses.AlreadyConnected:
                        Connected(this, new ConnectedEventArgs { Message = "Пользователь уже подключен", Result = false });
                        break;
                    case ConnectedStatuses.Connected:
                        Connected(this, new ConnectedEventArgs { Message = "Подключение удалось", Result = true });
                        break;
                    case ConnectedStatuses.Error:
                        Connected(this, new ConnectedEventArgs { Message = "Внутренняя ошибка", Result = false });
                        break;
                    case ConnectedStatuses.PasswordNotValid:
                        Connected(this, new ConnectedEventArgs { Message = "Неверный пароль", Result = false });
                        break;
                }
                if (e.Result != ConnectedStatuses.Connected) _chatClient.Close();
            }
            catch
            {
                Connected(this, new ConnectedEventArgs { Message = "Ошибка подключения", Result = false });
                _chatClient.Close();
            }
        }

        void GetAllUsersCompleted(object sender, GetAllUsersCompletedEventArgs e)
        {
            try
            {
                GetUsers(this, new UsersEventArgs { Users = e.Result });
            }
            catch (Exception ex)
            {
                Exception(this, new ExceptionEventArgs { Message = ex.Message });
            }
        }

        void ChatClientGetLogByUserCompleted(object sender, GetLogByUserCompletedEventArgs e)
        {
            try
            {
                GetLog(this, new LogEventArgs { Log = e.Result });
            }
            catch (Exception ex)
            {
                Exception(this, new ExceptionEventArgs { Message = ex.Message });
            }
        }

        void ChatClientGetLogCompleted(object sender, GetLogCompletedEventArgs e)
        {
            try
            {
                GetLog(this, new LogEventArgs { Log = e.Result });
            }
            catch (Exception ex)
            {
                Exception(this, new ExceptionEventArgs { Message = ex.Message });
            }
        }

        void ChatClientGetLogByRangeDateCompleted(object sender, GetLogByRangeDateCompletedEventArgs e)
        {
            try
            {
                GetLog(this, new LogEventArgs { Log = e.Result });
            }
            catch (Exception ex)
            {
                Exception(this, new ExceptionEventArgs { Message = ex.Message });
            }
        }

        public void GetAllUsers()
        {
            try
            {
                _chatClient.GetAllUsersAsync();
            }
            catch (Exception ex)
            {
                Exception(this, new ExceptionEventArgs { Message = ex.Message });
            }
        }

        public void Send(MessageDTO message)
        {
            try
            {
                _chatClient.SayAsync(message);
            }
            catch (Exception ex)
            {
                Exception(this, new ExceptionEventArgs { Message = ex.Message });
            }
        }

        public void RefreshClients(List<UserDTO> users)
        {
            try
            {
                RefreshUsers(this, new UsersEventArgs { Users = users });
            }
            catch (Exception ex)
            {
                Exception(this, new ExceptionEventArgs { Message = ex.Message });
            }
        }

        public void Receive(MessageDTO msg)
        {
            try
            {
                ReceiveMessage(this, new ReceiveEventArgs { Message = msg });
            }
            catch (Exception ex)
            {
                Exception(this, new ExceptionEventArgs { Message = ex.Message });
            }
        }

        public void ReturnUser(UserDTO msg)
        {
            try
            {
                ReturnedUser(this, new ReturnUserEventArgs { User = msg });
            }
            catch (Exception ex)
            {
                Exception(this, new ExceptionEventArgs { Message = ex.Message });
            }
        }

        public void Disconnect(UserDTO user)
        {
            try
            {
                _chatClient.DisconnectAsync(user);
            }
            catch (Exception ex)
            {
                Exception(this, new ExceptionEventArgs { Message = ex.Message });
            }
        }

        public void GetFullLog()
        {
            try
            {
                _chatClient.GetLogAsync();
            }
            catch (Exception ex)
            {
                Exception(this, new ExceptionEventArgs { Message = ex.Message });
            }
        }

        public void GetLogByDateRange(DateTime from, DateTime to)
        {
            try
            {
                _chatClient.GetLogByRangeDateAsync(from, to);
            }
            catch (Exception ex)
            {
                Exception(this, new ExceptionEventArgs { Message = ex.Message });
            }
        }

        public void GetLogByUser(UserDTO user)
        {
            try
            {
                _chatClient.GetLogByUserAsync(user);
            }
            catch (Exception ex)
            {
                Exception(this, new ExceptionEventArgs { Message = ex.Message });
            }
        }
        #region IChatCallback
        public IAsyncResult BeginRefreshClients(List<UserDTO> users, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }
        public void EndRefreshClients(IAsyncResult result)
        {
            throw new NotImplementedException();
        }
        public IAsyncResult BeginReceive(MessageDTO msg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }
        public void EndReceive(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginReturnUser(UserDTO msg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndReturnUser(IAsyncResult result)
        {
            throw new NotImplementedException();
        }
        #endregion IChatCallback
    }
}
