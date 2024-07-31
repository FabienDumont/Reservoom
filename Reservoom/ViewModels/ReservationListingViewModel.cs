using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Navigation;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using MVVMEssentials.ViewModels;
using Reservoom.Commands;
using Reservoom.Models;
using Reservoom.Services;
using Reservoom.Stores;

namespace Reservoom.ViewModels;

public class ReservationListingViewModel : BaseVm {
	private readonly HotelStore _hotelStore;
	
	private readonly ObservableCollection<ReservationViewModel> _reservations;
	public IEnumerable<ReservationViewModel> Reservations => _reservations;

	private bool _isLoading;

	public bool IsLoading {
		get => _isLoading;
		set {
			_isLoading = value;
			OnPropertyChanged();
		}
	}

	public ICommand LoadReservationsCommand { get; }
	public ICommand MakeReservationCommand { get; }

	public ReservationListingViewModel(HotelStore hotelStore, INavigationService makeReservationNavigationService) {
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

	public static ReservationListingViewModel LoadViewModel(HotelStore hotelStore, INavigationService makeReservationNavigationService) {
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