using System.Windows.Input;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using MVVMEssentials.ViewModels;
using Reservoom.Commands;
using Reservoom.Models;
using Reservoom.Services;
using Reservoom.Stores;

namespace Reservoom.ViewModels;

public class MakeReservationViewModel : BaseVm {
	private string _username;

	public string Username {
		get => _username;
		set {
			_username = value;
			OnPropertyChanged();
		}
	}

	private int _floorNumber;

	public int FloorNumber {
		get => _floorNumber;
		set {
			_floorNumber = value;
			OnPropertyChanged();
		}
	}

	private int _roomNumber;

	public int RoomNumber {
		get => _roomNumber;
		set {
			_roomNumber = value;
			OnPropertyChanged();
		}
	}

	private DateTime _startDate = new(2021, 1, 1);

	public DateTime StartDate {
		get => _startDate;
		set {
			_startDate = value;
			OnPropertyChanged();
		}
	}

	private DateTime _endDate = new(2021, 1, 8);

	public DateTime EndDate {
		get => _endDate;
		set {
			_endDate = value;
			OnPropertyChanged();
		}
	}

	public ICommand SubmitCommand { get; }
	public ICommand CancelCommand { get; }

	public MakeReservationViewModel(HotelStore hotelStore, INavigationService reservationListingNavigationService) {
		SubmitCommand = new MakeReservationCommand(this, hotelStore, reservationListingNavigationService);
		CancelCommand = new NavigateCommand(reservationListingNavigationService);
	}
}