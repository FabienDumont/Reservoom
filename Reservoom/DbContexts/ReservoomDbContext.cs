using Microsoft.EntityFrameworkCore;
using Reservoom.Dtos;

namespace Reservoom.DbContexts;

public class ReservoomDbContext : DbContext {
	public ReservoomDbContext(DbContextOptions options) : base(options) {
		
	}

	public DbSet<ReservationDto> Reservations { get; set; }
}