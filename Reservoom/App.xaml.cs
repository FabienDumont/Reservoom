using System.Windows;
using Microsoft.EntityFrameworkCore;
using MVVMEssentials.Services;
using MVVMEssentials.Stores;
using Reservoom.DbContexts;
using Reservoom.Models;
using Reservoom.Services.ReservationConflictValidator;
using Reservoom.Services.ReservationCreators;
using Reservoom.Services.ReservationProviders;
using Reservoom.Stores;
using Reservoom.ViewModels;

namespace Reservoom;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application {
	private const string ConnectionString = "Data Source=reservoom.db";
	private readonly Hotel _hotel;
	private readonly NavigationStore _navigationStore = new();
	private readonly ReservoomDbContextFactory _reservoomDbContextFactory;
	private readonly HotelStore _hotelStore;

	public App() {
		_reservoomDbContextFactory = new(ConnectionString);
		IReservationProvider reservationProvider = new DatabaseReservationProvider(_reservoomDbContextFactory);
		IReservationCreator reservationCreator = new DatabaseReservationCreator(_reservoomDbContextFactory);
		IReservationConflictValidator reservationConflictValidator = new DatabaseReservationConflictValidator(_reservoomDbContextFactory);
		
		ReservationBook reservationBook = new(reservationProvider, reservationCreator, reservationConflictValidator);
		
		_hotel = new Hotel("Fabibi suites", reservationBook);
		_hotelStore = new HotelStore(_hotel);
	}
	
	protected override void OnStartup(StartupEventArgs e) {
		using (ReservoomDbContext dbContext = _reservoomDbContextFactory.CreateDbContext()) {
			dbContext.Database.Migrate();
		}
		
		_navigationStore.CurrentViewModel = CreateReservationListingViewModel();
		
		MainWindow = new MainWindow {
			DataContext = new MainViewModel(_navigationStore)
		};
		
		MainWindow.Show();

		base.OnStartup(e);
	}

	private MakeReservationViewModel CreateMakeReservationViewModel() {
		return new MakeReservationViewModel(_hotelStore, new NavigationService<ReservationListingViewModel>(_navigationStore, CreateReservationListingViewModel));
	}

	private ReservationListingViewModel CreateReservationListingViewModel() {
		return ReservationListingViewModel.LoadViewModel(_hotelStore, new NavigationService<MakeReservationViewModel>(_navigationStore, CreateMakeReservationViewModel));
	}
}