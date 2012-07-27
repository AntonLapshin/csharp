
namespace ChatApplication.ViewModel.FilterStates
{
    public abstract class LogFilterState
    {
        public string Name { get; set; }
        
        public override string ToString()
        {
            return Name;
        }

        protected LogFilterState(LogViewModel vm)
        {
        }

        public abstract void SetState();
        public abstract void GetLog();
    }
}
