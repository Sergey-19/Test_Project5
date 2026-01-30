namespace EQX.Core.Common
{
    public class ViewModelProvider
    {
        public ViewModelProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public TViewModel GetViewModel<TViewModel>() where TViewModel : ViewModelBase
        {
            if (_serviceProvider.GetService(typeof(TViewModel)) == null)
            {
                throw new Exception($"{typeof(TViewModel)} could not be solved");
            }
            return (TViewModel)_serviceProvider.GetService(typeof(TViewModel))!;
        }
        #region Privates
        private readonly IServiceProvider _serviceProvider;
        #endregion
    }

}
