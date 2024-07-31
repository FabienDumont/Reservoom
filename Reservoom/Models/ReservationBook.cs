using Reservoom.Exceptions;
using Reservoom.Services.ReservationConflictValidator;
using Reservoom.Services.ReservationCreators;
using Reservoom.Services.ReservationProviders;

namespace Reservoom.Models;

public class ReservationBook {
	private readonly IReservationProvider _reservationProvider;
	private readonly IReservationCreator _reservationCreator;
	private readonly IReservationConflictValidator _reservationConflictValidator;

	public ReservationBook(IReservationProvider reservationProvider, IReservationCreator reservationCreator, IReservationConflictValidator reservationConflictValidator) {
		_reservationProvider = reservationProvider;
		_reservationCreator = reservationCreator;
		_reservationConflictValidator = reservationConflictValidator;
	}

	/// <summary>
	/// Get all reservations.
	/// </summary>
	/// <returns>The reservations.</returns>
	public async Task<IEnumerable<Reservation>> GetAllReservations() {
		return await _reservationProvider.GetAllReservations();
	}

	/// <summary>
	/// Add a reservation to the reservation book.
	/// </summary>
	/// <param name="reservation">The incoming reservation.</param>
	/// <exception cref="ReservationConflictException">Thrown if incoming reservation conflicts with existing reservation.</exception>
	public async Task AddReservation(Reservation reservation) {
		Reservation? conflictingReservation = _reservationConflictValidator.GetConflictingReservation(reservation).Result;

		if (conflictingReservation is not null) {
			throw new ReservationConflictException(conflictingReservation, reservation);
		}

		await _reservationCreator.CreateReservation(reservation);
	}
}