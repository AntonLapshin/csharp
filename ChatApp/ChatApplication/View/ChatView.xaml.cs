using GalaSoft.MvvmLight.Messaging;

namespace ChatApplication.View
{
    /// <summary>
    /// Логика взаимодействия для ChatView.xaml
    /// </summary>
    public partial class ChatView
    {
        private const string ShowLogView = "LogView";
        private const string CloseChat = "ChatViewModelClose";
        public ChatView()
        {
            InitializeComponent();
            Messenger.Default.Register<NotificationMessage>(this, NotificationMessageReceived);
        }

        private void NotificationMessageReceived(NotificationMessage msg)
        {
            switch (msg.Notification)
            {
                case ShowLogView:
                    {
                        var logView = new LogView();
                        logView.ShowDialog();
                    }
                    break;
                case CloseChat:
                    Close();
                    break;
            }
        }
    }
}
