using MVVMEssentials.ViewModels;
using Reservoom.Models;

namespace Reservoom.ViewModels;

public class ReservationViewModel : BaseVm {
	private readonly Reservation _reservation;

	public string RoomId => _reservation.RoomId.ToString();
	public string Username => _reservation.Username;
	public string StartDate => _reservation.StartDate.ToString("d");
	public string EndDate => _reservation.EndDate.ToString("d");

	public ReservationViewModel(Reservation reservation) {
		_reservation = reservation;
	}
}