using Reservoom.Services;
using Reservoom.Stores;
using Reservoom.ViewModels;

namespace Reservoom.Commands;

public class NavigateCommand : CommandBase {
	private readonly NavigationService _navigationService;

	public NavigateCommand(NavigationService navigationService) {
		_navigationService = navigationService;
	}
    
	public override void Execute(object? parameter) {
		_navigationService.Navigate();
	}
}