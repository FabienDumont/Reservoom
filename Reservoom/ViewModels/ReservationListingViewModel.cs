using System.Collections.ObjectModel;
using System.Windows.Input;
using Reservoom.Commands;
using Reservoom.Models;
using Reservoom.Services;
using Reservoom.Stores;

namespace Reservoom.ViewModels;

public class ReservationListingViewModel : ViewModelBase {
	private readonly HotelStore _hotelStore;
	
	private readonly ObservableCollection<ReservationViewModel> _reservations;
	public IEnumerable<ReservationViewModel> Reservations => _reservations;

	public ICommand LoadReservationsCommand { get; }
	public ICommand MakeReservationCommand { get; }

	public ReservationListingViewModel(HotelStore hotelStore, NavigationService makeReservationNavigationService) {
		_hotelStore = hotelStore;
		_reservations = new ObservableCollection<ReservationViewModel>();

		LoadReservationsCommand = new LoadReservationsCommand(this, hotelStore);
		MakeReservationCommand = new NavigateCommand(makeReservationNavigationService);

		_hotelStore.ReservationMade += OnReservationMade;
	}

	public override void Dispose() {
		_hotelStore.ReservationMade -= OnReservationMade;
		base.Dispose();
	}

	private void OnReservationMade(Reservation reservation) {
		ReservationViewModel reservationViewModel = new(reservation);
		_reservations.Add(reservationViewModel);
	}

	public static ReservationListingViewModel LoadViewModel(HotelStore hotelStore, NavigationService makeReservationNavigationService) {
		ReservationListingViewModel viewModel = new(hotelStore, makeReservationNavigationService);
		
		viewModel.LoadReservationsCommand.Execute(null);

		return viewModel;
	}

	public void UpdateReservations(IEnumerable<Reservation> reservations) {
		_reservations.Clear();

		foreach (Reservation reservation in reservations) {
			ReservationViewModel reservationViewModel = new(reservation);
			_reservations.Add(reservationViewModel);
		}
	}
}