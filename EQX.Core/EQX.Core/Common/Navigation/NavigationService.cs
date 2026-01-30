using CommunityToolkit.Mvvm.ComponentModel;

namespace EQX.Core.Common
{
    public class NavigationService : ObservableObject, INavigationService
    {
        public event EventHandler? Navigating;

        public NavigationService(ViewModelNavigationStore viewModelavigationStore,
            ViewModelProvider viewModelProvider)
        {
            _viewModelavigationStore = viewModelavigationStore;
            _viewModelProvider = viewModelProvider;
        }

        public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        {
            Navigating?.Invoke(this, EventArgs.Empty);

            var viewModel = _viewModelProvider.GetViewModel<TViewModel>();
            _viewModelavigationStore.CurrentViewModel = viewModel;
        }

        #region Privates
        private readonly ViewModelProvider _viewModelProvider;
        private readonly ViewModelNavigationStore _viewModelavigationStore;
        #endregion
    }

}
