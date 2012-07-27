using System.Windows;
using GalaSoft.MvvmLight.Messaging;

namespace ChatApplication.View
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationView.xaml
    /// </summary>
    public partial class AuthorizationView
    {
        private const string ChatViewShow = "ChatView";
        public AuthorizationView()
        {
            InitializeComponent();
            Messenger.Default.Register<NotificationMessage>(this, NotificationMessageReceived);
            Messenger.Default.Register<DialogMessage>(
                this,
                msg => MessageBox.Show(
                    msg.Content
                    ));
        }

        private void NotificationMessageReceived(NotificationMessage msg)
        {
            if (msg.Notification != ChatViewShow) return;
            var chatView = new ChatView();
            Hide();
            chatView.ShowDialog();
            Close();
        }
    }
}
