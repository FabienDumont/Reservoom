using MVVMEssentials.Stores;
using MVVMEssentials.ViewModels;

namespace Reservoom.ViewModels;

public class MainViewModel : BaseVm {
	private readonly NavigationStore _navigationStore;
	public BaseVm? CurrentViewModel => _navigationStore.CurrentViewModel;

	public MainViewModel(NavigationStore navigationStore) {
		_navigationStore = navigationStore;
		
		_navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
	}

	private void OnCurrentViewModelChanged() {
		OnPropertyChanged(nameof(CurrentViewModel));
	}
}