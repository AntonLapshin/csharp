using ChatApplication.Model;

namespace ChatApplication.ViewModel
{
    public class ViewModelLocator
    {
        private static AuthorizationViewModel _authorization;
        private static ChatViewModel _chat;
        private static LogViewModel _log;

        private readonly ChatProxy _proxy;
        
        public ViewModelLocator()
        {
            _proxy = new ChatProxy();
            
            _authorization = new AuthorizationViewModel(_proxy);
            _chat = new ChatViewModel(_proxy);
            _log = new LogViewModel(_proxy);
        }

        /// <summary>
        /// Gets the Main property which defines the main viewmodel.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        
        public AuthorizationViewModel Authorization
        {
            get { return _authorization; }
        }

        public ChatViewModel Chat
        {
            get { return _chat; }
        }

        public LogViewModel Log
        {
            get { return _log; }
        }
    }
}