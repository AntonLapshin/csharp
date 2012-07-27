using System.Windows;

namespace ChatApplication.ViewModel.FilterStates
{
    public class FilterByUserState : LogFilterState
    {
        private readonly LogViewModel _logViewModel;

        public FilterByUserState(LogViewModel viewModel)
            : base(viewModel)
        {
            _logViewModel = viewModel;
            Name = "По пользователю";
        }
        public override void SetState()
        {
            _logViewModel.VisibilityFilterByRangeDate = Visibility.Collapsed;
            _logViewModel.VisibilityFilterByUser = Visibility.Visible;
        }

        public override void GetLog()
        {
            _logViewModel.GetLogByUser();
        }
    }
}
