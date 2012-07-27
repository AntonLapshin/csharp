using System.Windows;

namespace ChatApplication.ViewModel.FilterStates
{
    public class NormalFilterState : LogFilterState
    {
        private readonly LogViewModel _logViewModel;

        public NormalFilterState(LogViewModel viewModel)
            : base(viewModel)
        {
            _logViewModel = viewModel;
            Name = "Весь лог";
        }
        public override void SetState()
        {
            _logViewModel.VisibilityFilterByRangeDate = Visibility.Collapsed;
            _logViewModel.VisibilityFilterByUser = Visibility.Collapsed;
        }
        public override void GetLog()
        {
            _logViewModel.GetFullLog();
        }
    }
}
