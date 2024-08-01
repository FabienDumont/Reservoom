using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MVVMEssentials.Services;
using MVVMEssentials.Stores;
using Reservoom.DbContexts;
using Reservoom.Dtos;
using Reservoom.Models;
using Reservoom.Services.ReservationConflictValidator;
using Reservoom.Services.ReservationCreators;
using Reservoom.Services.ReservationProviders;
using Reservoom.Stores;
using Reservoom.ViewModels;

namespace Reservoom.Tests.ViewModels;

public class MakeReservationViewModelTest {
	[Test]
	public async Task ExecuteSubmitCommand_WithValidReservation_CreatesReservation() {
		ServiceCollection services = new();

		services.AddSingleton(
			s => ReservationListingViewModel.LoadViewModel(
				s.GetRequiredService<HotelStore>(), s.GetRequiredService<NavigationService<MakeReservationViewModel>>()
			)
		);
		services.AddSingleton(
			s => new NavigationService<ReservationListingViewModel>(s.GetRequiredService<NavigationStore>(), s.GetRequiredService<ReservationListingViewModel>)
		);

		services.AddSingleton<MakeReservationViewModel>(
			s => new MakeReservationViewModel(s.GetRequiredService<HotelStore>(), s.GetRequiredService<NavigationService<ReservationListingViewModel>>())
		);
		services.AddSingleton(
			s => new NavigationService<MakeReservationViewModel>(s.GetRequiredService<NavigationStore>(), s.GetRequiredService<MakeReservationViewModel>)
		);

		services.AddSingleton<HotelStore>();
		services.AddSingleton<Hotel>(s => new Hotel("Hotel Name", s.GetRequiredService<ReservationBook>()));
		services.AddSingleton<ReservationBook>();
		services.AddSingleton<IReservationProvider, DatabaseReservationProvider>();
		services.AddSingleton<IReservationCreator, DatabaseReservationCreator>();
		services.AddSingleton<IReservationConflictValidator, DatabaseReservationConflictValidator>();
		services.AddSingleton<IReservoomDbContextFactory, InMemoryReservoomDbContextFactory>();
		services.AddSingleton<NavigationStore>();

		ServiceProvider serviceProvider = services.BuildServiceProvider();

		IReservoomDbContextFactory dbContextFactory = serviceProvider.GetRequiredService<IReservoomDbContextFactory>();
		ReservoomDbContext migrationDbContext = dbContextFactory.CreateDbContext();
		await migrationDbContext.Database.MigrateAsync();

		MakeReservationViewModel viewModel = serviceProvider.GetRequiredService<MakeReservationViewModel>();

		viewModel.Username = "Fabibi";
		viewModel.FloorNumber = 1;
		viewModel.RoomNumber = 2;
		viewModel.StartDate = new DateTime(2000, 1, 1);
		viewModel.EndDate = new DateTime(2000, 1, 2);

		viewModel.SubmitCommand.Execute(null);

		ReservoomDbContext dbContext = dbContextFactory.CreateDbContext();
		ReservationDto createdReservation = await dbContext.Reservations.FirstOrDefaultAsync(
			                                    r => r.Username == "Fabibi" &&
			                                         r.FloorNumber == 1 &&
			                                         r.RoomNumber == 2 &&
			                                         r.StartDate == new DateTime(2000, 1, 1) &&
			                                         r.EndDate == new DateTime(2000, 1, 2)
		                                    ) ??
		                                    throw new InvalidOperationException();

		Assert.That(createdReservation, Is.Not.Null);
	}
}