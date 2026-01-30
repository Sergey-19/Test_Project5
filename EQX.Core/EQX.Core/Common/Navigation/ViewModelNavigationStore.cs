namespace EQX.Core.Common
{
    public class ViewModelNavigationStore
    {
        public event Action? CurrentViewModelChanged;

        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel?.Dispose();
                _currentViewModel = value;

                CurrentViewModelChanged?.Invoke();
            }
        }

        #region Privates
        private ViewModelBase _currentViewModel;
        #endregion
    }

}
