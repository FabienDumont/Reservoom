using Microsoft.EntityFrameworkCore;

namespace Reservoom.DbContexts;

public class ReservoomDbContextFactory : IReservoomDbContextFactory {
	private readonly string _connectionString;

	public ReservoomDbContextFactory(string connectionString) {
		_connectionString = connectionString;
	}

	public ReservoomDbContext CreateDbContext() {
		DbContextOptions options = new DbContextOptionsBuilder().UseSqlite(_connectionString).Options;

		return new ReservoomDbContext(options);
	}
}