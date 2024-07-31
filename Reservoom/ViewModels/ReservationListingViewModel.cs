using System.Collections.ObjectModel;
using System.Windows.Input;
using Reservoom.Commands;
using Reservoom.Models;
using Reservoom.Services;
using Reservoom.Stores;

namespace Reservoom.ViewModels;

public class ReservationListingViewModel : ViewModelBase {
	private readonly ObservableCollection<ReservationViewModel> _reservations;
	public IEnumerable<ReservationViewModel> Reservations => _reservations;

	public ICommand LoadReservationsCommand { get; }
	public ICommand MakeReservationCommand { get; }

	public ReservationListingViewModel(Hotel hotel, NavigationService makeReservationNavigationService) {
		_reservations = new ObservableCollection<ReservationViewModel>();

		LoadReservationsCommand = new LoadReservationsCommand(this, hotel);
		MakeReservationCommand = new NavigateCommand(makeReservationNavigationService);
	}

	public static ReservationListingViewModel LoadViewModel(Hotel hotel, NavigationService makeReservationNavigationService) {
		ReservationListingViewModel viewModel = new(hotel, makeReservationNavigationService);
		
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