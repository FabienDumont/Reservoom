using System.ComponentModel;
using System.Windows;
using Reservoom.Exceptions;
using Reservoom.Models;
using Reservoom.Services;
using Reservoom.ViewModels;

namespace Reservoom.Commands;

public class MakeReservationCommand : CommandBase {
	private readonly MakeReservationViewModel _makeReservationViewModel;
	private readonly Hotel _hotel;
	private readonly NavigationService _reservationListingNavigationService;

	public MakeReservationCommand(MakeReservationViewModel makeReservationViewModel, Hotel hotel, NavigationService reservationListingNavigationService) {
		_makeReservationViewModel = makeReservationViewModel;
		_hotel = hotel;
		_reservationListingNavigationService = reservationListingNavigationService;

		_makeReservationViewModel.PropertyChanged += OnViewModelPropertyChanged;
	}

	public override bool CanExecute(object? parameter) {
		return !string.IsNullOrEmpty(_makeReservationViewModel.Username) && _makeReservationViewModel.FloorNumber > 0 && base.CanExecute(parameter);
	}

	public override void Execute(object? parameter) {
		Reservation reservation = new(
			new RoomId(_makeReservationViewModel.FloorNumber, _makeReservationViewModel.RoomNumber), _makeReservationViewModel.Username,
			_makeReservationViewModel.StartDate, _makeReservationViewModel.EndDate
		);

		try {
			_hotel.MakeReservation(reservation);
			MessageBox.Show("Successfully reserved room.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

			_reservationListingNavigationService.Navigate();
		} catch (ReservationConflictException) {
			MessageBox.Show("This room is already taken.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e) {
		if (e.PropertyName is nameof(MakeReservationViewModel.Username) or nameof(MakeReservationViewModel.FloorNumber)) {
			OnCanExecuteChanged();
		}
	}
}