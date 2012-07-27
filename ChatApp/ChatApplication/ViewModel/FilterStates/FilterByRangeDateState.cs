using System.Windows;

namespace ChatApplication.ViewModel.FilterStates
{
    public class FilterByRangeDateState : LogFilterState
    {
        private readonly LogViewModel _logViewModel;

        public FilterByRangeDateState(LogViewModel viewModel)
            : base(viewModel)
        {
            _logViewModel = viewModel;
            Name = "По дате";
        }
        public override void SetState()
        {
            _logViewModel.VisibilityFilterByRangeDate = Visibility.Visible;
            _logViewModel.VisibilityFilterByUser = Visibility.Collapsed;
        }

        public override void GetLog()
        {
            _logViewModel.GetLogByDateRange();
        }
    }
}
