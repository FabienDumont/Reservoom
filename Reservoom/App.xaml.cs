using System.Configuration;
using System.Data;
using System.Windows;
using Reservoom.Exceptions;
using Reservoom.Models;
using Reservoom.Services;
using Reservoom.Stores;
using Reservoom.ViewModels;

namespace Reservoom;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application {
	private readonly Hotel _hotel;
	private readonly NavigationStore _navigationStore = new();

	public App() {
		_hotel = new Hotel("Fabibi suites");
	}
	
	protected override void OnStartup(StartupEventArgs e) {
		_navigationStore.CurrentViewModel = CreateReservationListingViewModel();
		
		MainWindow = new MainWindow {
			DataContext = new MainViewModel(_navigationStore)
		};
		
		MainWindow.Show();

		base.OnStartup(e);
	}

	private MakeReservationViewModel CreateMakeReservationViewModel() {
		return new MakeReservationViewModel(_hotel, new NavigationService(_navigationStore, CreateReservationListingViewModel));
	}

	private ReservationListingViewModel CreateReservationListingViewModel() {
		return new ReservationListingViewModel(_hotel, new NavigationService(_navigationStore, CreateMakeReservationViewModel));
	}
}