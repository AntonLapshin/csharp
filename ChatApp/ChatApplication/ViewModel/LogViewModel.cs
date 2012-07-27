using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using ChatApplication.ChatServiceReference;
using ChatApplication.Model;
using ChatApplication.ViewModel.FilterStates;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace ChatApplication.ViewModel
{
    public class LogViewModel : ViewModelBase
    {
        private readonly ChatProxy _proxy;
        private LogFilterState _currentState;
        private DateTime _dateFrom;
        private DateTime _dateTo;
        private Visibility _visibilityFilterByRangeDate;
        private Visibility _visibilityFilterByUser;
        private UserDTO _user;
        private ObservableCollection<UserDTO> _users;
        private ObservableCollection<MessageDTO> _messages;

        public RelayCommand GetLogCommand { get; set; }
        public RelayCommand GetUsersCommand { get; set; }

        public LogFilterState CurrentState
        {
            get { return _currentState; }
            set
            {
                _currentState = value;
                _currentState.SetState();
                RaisePropertyChanged("CurrentState");
            }
        }

        public Visibility VisibilityFilterByUser
        {
            get { return _visibilityFilterByUser; }
            set
            {
                _visibilityFilterByUser = value;
                RaisePropertyChanged("VisibilityFilterByUser");
            }
        }

        public Visibility VisibilityFilterByRangeDate
        {
            get { return _visibilityFilterByRangeDate; }
            set
            {
                _visibilityFilterByRangeDate = value;
                RaisePropertyChanged("VisibilityFilterByRangeDate");
            }
        }

        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set
            {
                _dateFrom = value;
                RaisePropertyChanged("DateFrom");
            }
        }

        public DateTime DateTo
        {
            get { return _dateTo; }
            set
            {
                _dateTo = value;
                RaisePropertyChanged("DateTo");
            }
        }

        public UserDTO CurrentUser
        {
            get { return _user; }
            set
            {
                _user = value;
                RaisePropertyChanged("User");
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

        public List<LogFilterState> LogFilterStates
        {
            get
            {
                return new List<LogFilterState> { new NormalFilterState(this), new FilterByUserState(this), new FilterByRangeDateState(this) };
            }
        }

        public LogViewModel(ChatProxy proxy)
        {
            CurrentState = new NormalFilterState(this);
            Messages = new ObservableCollection<MessageDTO>();
            _proxy = proxy;
            _proxy.GetUsers += ProxyGetUsers;
            _proxy.GetLog += ProxyGetLog;
            GetLogCommand = new RelayCommand(ActionGetLog);
            GetUsersCommand = new RelayCommand(GetUsers);
        }

        void ProxyGetLog(object sender, LogEventArgs e)
        {
            Messages = new ObservableCollection<MessageDTO>(e.Log);
        }

        void ProxyGetUsers(object sender, UsersEventArgs e)
        {
            Users = new ObservableCollection<UserDTO>(e.Users);
        }

        private void GetUsers()
        {
            _proxy.GetAllUsers();
        }

        private void ActionGetLog()
        {
            CurrentState.GetLog();
        }

        public void GetLogByUser()
        {
            _proxy.GetLogByUser(CurrentUser);
        }

        public void GetFullLog()
        {
            _proxy.GetFullLog();
        }

        public void GetLogByDateRange()
        {
            _proxy.GetLogByDateRange(DateFrom, DateTo);
        }
    }
}
