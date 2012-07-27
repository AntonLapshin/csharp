using System;
using System.Collections.ObjectModel;
using ChatApplication.ChatServiceReference;
using ChatApplication.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace ChatApplication.ViewModel
{
    public class ChatViewModel : ViewModelBase
    {
        private const string ShowLogView = "LogView";
        private const string CloseChat = "ChatViewModelClose";
        private readonly ChatProxy _proxy;
        private UserDTO _currentUser;
        private string _currentMessage;
        private ObservableCollection<UserDTO> _users;
        private ObservableCollection<MessageDTO> _messages;

        public RelayCommand LogShowCommand { get; set; }
        public RelayCommand SendMessageCommand { get; set; }
        public RelayCommand ClosedCommand { get; set; }

        public UserDTO CurrentUser
        {
            get
            {
                return _currentUser;
            }
            set
            {
                _currentUser = value;
                RaisePropertyChanged("CurrentUser");
            }
        }
        
        public ObservableCollection<UserDTO> Users
        {
            get
            {
                return _users;
            }
            set
            {
                _users = value;
                RaisePropertyChanged("Users");
            } 
        }

        public ObservableCollection<MessageDTO> Messages
        {
            get
            {
                return _messages;
            }
            set
            {
                _messages = value;
                RaisePropertyChanged("Messages");
            }
        }
        
        public string CurrentMessage
        {
            get
            {
                return _currentMessage;
            }
            set
            {
                _currentMessage = value; 
                RaisePropertyChanged("CurrentMessage");
            }
        }

        public ChatViewModel(ChatProxy proxy)
        {
            _proxy = proxy;
            _proxy.RefreshUsers += ProxyRefreshUsers;
            _proxy.ReceiveMessage += ProxyRecieveMessage;
            _proxy.ReturnedUser += ProxyReturnedUser;

            LogShowCommand = new RelayCommand(LogShow);
            SendMessageCommand = new RelayCommand(SendMessage);
            ClosedCommand = new RelayCommand(ClosedWindow);

            Users=new ObservableCollection<UserDTO>();
            Messages = new ObservableCollection<MessageDTO>();
        }

        private void ProxyReturnedUser(object sender, ReturnUserEventArgs e)
        {
            _currentUser = e.User;
        }
        
        private void ProxyRecieveMessage(object sender, ReceiveEventArgs e)
        {
            Messages.Add(e.Message);
        }

        private void ProxyRefreshUsers(object sender, UsersEventArgs e)
        {
            Users = new ObservableCollection<UserDTO>(e.Users);
        }

        private static void LogShow()
        {
            Messenger.Default.Send(new NotificationMessage(ShowLogView));
        }

        private void SendMessage()
        {
            if (string.IsNullOrWhiteSpace(CurrentMessage)) return;
            var msg = new MessageDTO { MessageText = CurrentMessage, Timestamp = DateTime.Now, User = _currentUser, UserNum = _currentUser.Id};
            _proxy.Send(msg);
            CurrentMessage = "";
        }

        private void ClosedWindow()
        {
            _proxy.Disconnect(_currentUser);
            Messenger.Default.Send(new NotificationMessage(CloseChat));
        }
    }
}
