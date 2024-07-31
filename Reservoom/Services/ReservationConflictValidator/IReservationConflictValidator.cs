using Reservoom.Models;

namespace Reservoom.Services.ReservationConflictValidator;

public interface IReservationConflictValidator {
	Task<Reservation?> GetConflictingReservation(Reservation reservation);
}