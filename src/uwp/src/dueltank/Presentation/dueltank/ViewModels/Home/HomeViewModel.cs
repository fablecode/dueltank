using dueltank.ViewModels.Infrastructure.ViewModels;

namespace dueltank.ViewModels.Home
{
    public class HomeViewModel : ViewModelBase
    {
        private bool _isBusy = true;

        public bool IsBusy
        {
            get => _isBusy;
            set => Set(ref _isBusy, value);
        }

    }
}