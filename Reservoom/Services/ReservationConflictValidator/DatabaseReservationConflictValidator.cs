using Microsoft.EntityFrameworkCore;
using Reservoom.DbContexts;
using Reservoom.Dtos;
using Reservoom.Models;

namespace Reservoom.Services.ReservationConflictValidator;

public class DatabaseReservationConflictValidator : IReservationConflictValidator {
	private readonly IReservoomDbContextFactory _dbContextFactory;

	public DatabaseReservationConflictValidator(IReservoomDbContextFactory dbContextFactory) {
		_dbContextFactory = dbContextFactory;
	}

	public async Task<Reservation?> GetConflictingReservation(Reservation reservation) {
		using (ReservoomDbContext context = _dbContextFactory.CreateDbContext()) {
			ReservationDto? reservationDto = await context.Reservations.Where(r => r.FloorNumber == reservation.RoomId.FloorNumber)
				.Where(r => r.RoomNumber == reservation.RoomId.RoomNumber).Where(r => r.EndDate > reservation.StartDate)
				.Where(r => r.StartDate < reservation.EndDate).FirstOrDefaultAsync();

			return reservationDto is null ? null : ToReservation(reservationDto);
		}
	}

	private static Reservation ToReservation(ReservationDto r) {
		return new Reservation(new RoomId(r.FloorNumber, r.RoomNumber), r.Username, r.StartDate, r.EndDate);
	}
}