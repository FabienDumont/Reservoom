using System.Collections;
using System.ComponentModel;
using System.Windows.Input;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using MVVMEssentials.ViewModels;
using Reservoom.Commands;
using Reservoom.Stores;

namespace Reservoom.ViewModels;

public class MakeReservationViewModel : BaseVm, INotifyDataErrorInfo {
	private string _username = "";

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
			
			ClearErrors(nameof(StartDate));
			ClearErrors(nameof(EndDate));
			
			if (EndDate < StartDate) {
				AddError("The start date cannot be after the end date.", nameof(StartDate));
			}
		}
	}

	private DateTime _endDate = new(2021, 1, 8);

	public DateTime EndDate {
		get => _endDate;
		set {
			_endDate = value;
			OnPropertyChanged();
			
			ClearErrors(nameof(StartDate));
			ClearErrors(nameof(EndDate));

			if (EndDate < StartDate) {
				AddError("The end date cannot be before the start date.", nameof(EndDate));
			}
		}
	}

	public ICommand SubmitCommand { get; }
	public ICommand CancelCommand { get; }
	private readonly Dictionary<string, List<string>> _propertyNameToErrorsDictionary;
	public bool HasErrors => _propertyNameToErrorsDictionary.Any();
	public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

	public MakeReservationViewModel(HotelStore hotelStore, INavigationService reservationListingNavigationService) {
		SubmitCommand = new MakeReservationCommand(this, hotelStore, reservationListingNavigationService);
		CancelCommand = new NavigateCommand(reservationListingNavigationService);

		_propertyNameToErrorsDictionary = new Dictionary<string, List<string>>();
	}
	
	private void AddError(string errorMessage, string propertyName) {
		if (!_propertyNameToErrorsDictionary.ContainsKey(propertyName)) {
			_propertyNameToErrorsDictionary.Add(propertyName, new List<string>());
		}
		
		_propertyNameToErrorsDictionary[propertyName].Add(errorMessage);
		
		OnErrorsChanged(propertyName);
	}
	
	private void ClearErrors(string propertyName) {
		_propertyNameToErrorsDictionary.Remove(propertyName);
		OnErrorsChanged(propertyName);
	}
	
	private void OnErrorsChanged(string propertyName) {
		ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
	}

	public IEnumerable GetErrors(string? propertyName) {
		return _propertyNameToErrorsDictionary!.GetValueOrDefault(propertyName, new List<string>());
	}
}