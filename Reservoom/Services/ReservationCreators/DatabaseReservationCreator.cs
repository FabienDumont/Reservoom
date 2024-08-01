using Reservoom.DbContexts;
using Reservoom.Dtos;
using Reservoom.Models;

namespace Reservoom.Services.ReservationCreators;

public class DatabaseReservationCreator : IReservationCreator {
	private readonly IReservoomDbContextFactory _dbContextFactory;

	public DatabaseReservationCreator(IReservoomDbContextFactory dbContextFactory) {
		_dbContextFactory = dbContextFactory;
	}
	
	public async Task CreateReservation(Reservation reservation) {
		using (ReservoomDbContext context = _dbContextFactory.CreateDbContext()) {
			ReservationDto reservationDto = ToReservationDto(reservation);

			context.Reservations.Add(reservationDto);
			await context.SaveChangesAsync();
		}
	}

	private ReservationDto ToReservationDto(Reservation reservation) {
		return new ReservationDto() {
			FloorNumber = reservation.RoomId.FloorNumber,
			RoomNumber = reservation.RoomId.RoomNumber,
			Username = reservation.Username,
			StartDate = reservation.StartDate,
			EndDate = reservation.EndDate
		};
	}
}