using System.ComponentModel;
using System.Windows;
using Reservoom.Exceptions;
using Reservoom.Models;
using Reservoom.Services;
using Reservoom.Stores;
using Reservoom.ViewModels;

namespace Reservoom.Commands;

public class MakeReservationCommand : AsyncCommandBase {
	private readonly MakeReservationViewModel _makeReservationViewModel;
	private readonly HotelStore _hotelStore;
	private readonly NavigationService _reservationListingNavigationService;

	public MakeReservationCommand(MakeReservationViewModel makeReservationViewModel, HotelStore hotelStore, NavigationService reservationListingNavigationService) {
		_makeReservationViewModel = makeReservationViewModel;
		_hotelStore = hotelStore;
		_reservationListingNavigationService = reservationListingNavigationService;

		_makeReservationViewModel.PropertyChanged += OnViewModelPropertyChanged;
	}

	public override bool CanExecute(object? parameter) {
		return !string.IsNullOrEmpty(_makeReservationViewModel.Username) && _makeReservationViewModel.FloorNumber > 0 && base.CanExecute(parameter);
	}

	public override async Task ExecuteAsync(object? parameter) {
		Reservation reservation = new(
			new RoomId(_makeReservationViewModel.FloorNumber, _makeReservationViewModel.RoomNumber), _makeReservationViewModel.Username,
			_makeReservationViewModel.StartDate, _makeReservationViewModel.EndDate
		);

		try {
			await _hotelStore.MakeReservation(reservation);
			
			MessageBox.Show("Successfully reserved room.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

			//_reservationListingNavigationService.Navigate();
		} catch (ReservationConflictException) {
			MessageBox.Show("This room is already taken.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		} catch (Exception) {
			MessageBox.Show("Failed to make reservation.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e) {
		if (e.PropertyName is nameof(MakeReservationViewModel.Username) or nameof(MakeReservationViewModel.FloorNumber)) {
			OnCanExecuteChanged();
		}
	}
}