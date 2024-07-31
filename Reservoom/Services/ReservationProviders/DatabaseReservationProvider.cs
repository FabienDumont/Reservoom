﻿using Microsoft.EntityFrameworkCore;
using Reservoom.DbContexts;
using Reservoom.Dtos;
using Reservoom.Models;

namespace Reservoom.Services.ReservationProviders;

public class DatabaseReservationProvider : IReservationProvider {
	private readonly ReservoomDbContextFactory _dbContextFactory;

	public DatabaseReservationProvider(ReservoomDbContextFactory dbContextFactory) {
		_dbContextFactory = dbContextFactory;
	}
	
	public async Task<IEnumerable<Reservation>> GetAllReservations() {
		using (ReservoomDbContext context = _dbContextFactory.CreateDbContext()) {
			IEnumerable<ReservationDto> reservationDtos = await context.Reservations.ToListAsync();

			return reservationDtos.Select(ToReservation);
		}
	}

	private static Reservation ToReservation(ReservationDto r) {
		return new Reservation(new RoomId(r.FloorNumber, r.RoomNumber), r.Username, r.StartDate, r.EndDate);
	}
}