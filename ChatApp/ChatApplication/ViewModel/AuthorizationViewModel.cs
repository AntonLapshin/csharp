using System.Windows;
using ChatApplication.ChatServiceReference;
using ChatApplication.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace ChatApplication.ViewModel
{
    public class AuthorizationViewModel : ViewModelBase
    {
        private const string ChatViewShow = "ChatView";
        private const string FieldsEmpty = "Не все поля заполнены!";

        private UserDTO _user;
        private string _host = "localhost";
        private string _port = "7997";
        private readonly ChatProxy _proxy;
        private bool _enterEnabled=true;
        
        public RelayCommand AuthorizationCommand { get; private set; }

        public UserDTO User
        {
            get { return _user; }
            set
            {
                _user = value;
                RaisePropertyChanged("User");
            }
        }

        public string Host
        {
            get
            {
                return _host;
            } 
            set
            {
                _host = value; 
                RaisePropertyChanged("Host");
            }
        }

        public string Port
        {
            get
            {
                return _port;
            } 
            set
            {
                _port = value; 
                RaisePropertyChanged("Port");
            }
        }

        public bool EnterEnabled
        {
            get
            {
                return _enterEnabled;
            }
            set
            {
                _enterEnabled = value;
                RaisePropertyChanged("EnterEnabled");
            }
        }

        public AuthorizationViewModel(ChatProxy proxy)
        {
            _user = new UserDTO();
            _proxy = proxy;
            _proxy.Connected += ProxyConnected;
            _proxy.Exception += ProxyException;
            AuthorizationCommand = new RelayCommand(Authorization);
        }

        private void ProxyException(object sender, ExceptionEventArgs e)
        {
            ActivateEnterButtonOnError();

            ShowMessageBox(e.Message);
        }

        private void ActivateEnterButtonOnError()
        {
            if (!EnterEnabled)
            {
                EnterEnabled = true;
            }
        }

        private void ProxyConnected(object sender, ConnectedEventArgs e)
        {
            if (e.Result)
            {
                Messenger.Default.Send(new NotificationMessage(ChatViewShow));
            }
            else
            {
                EnterEnabled = true;
                ShowMessageBox(e.Message);
            }
        }
        private void DialogMessageCallback(MessageBoxResult result)
        {
        }

        private void Authorization()
        {
            if (string.IsNullOrWhiteSpace(Host) || string.IsNullOrWhiteSpace(Port) || string.IsNullOrWhiteSpace(User.Name) || string.IsNullOrWhiteSpace(User.Password))
            {
                ShowMessageBox(FieldsEmpty);
                return;
            }
            EnterEnabled = false;
            _proxy.Connect(Host,Port,User);
        }

        private void ShowMessageBox(string message)
        {
            Messenger.Default.Send(new DialogMessage(message, DialogMessageCallback));
        }
    }
}
