using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVVMEssentials.Services;
using MVVMEssentials.Stores;
using Reservoom.DbContexts;
using Reservoom.HostBuilders;
using Reservoom.Models;
using Reservoom.Services.ReservationConflictValidator;
using Reservoom.Services.ReservationCreators;
using Reservoom.Services.ReservationProviders;
using Reservoom.Stores;
using Reservoom.ViewModels;

namespace Reservoom;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App {
	private readonly IHost _host;

	public App() {
		_host = Host.CreateDefaultBuilder().AddViewModels().ConfigureServices(
			(hostContext, services) => {
				string connectionString = hostContext.Configuration.GetConnectionString("Default") ?? throw new InvalidOperationException();
				
				services.AddSingleton(new ReservoomDbContextFactory(connectionString));
				services.AddSingleton<IReservationProvider, DatabaseReservationProvider>();
				services.AddSingleton<IReservationCreator, DatabaseReservationCreator>();
				services.AddSingleton<IReservationConflictValidator, DatabaseReservationConflictValidator>();

				services.AddTransient<ReservationBook>();
				
				string hotelName = hostContext.Configuration.GetValue<string>("HotelName") ?? throw new InvalidOperationException();
				services.AddSingleton<Hotel>(s => new Hotel(hotelName, s.GetRequiredService<ReservationBook>()));

				services.AddSingleton<HotelStore>();
				services.AddSingleton<NavigationStore>();

				services.AddSingleton<MainViewModel>();
				services.AddSingleton(
					s => new MainWindow {
						DataContext = s.GetRequiredService<MainViewModel>()
					}
				);
			}
		).Build();
	}

	protected override void OnStartup(StartupEventArgs e) {
		_host.Start();

		ReservoomDbContextFactory reservoomDbContextFactory = _host.Services.GetRequiredService<ReservoomDbContextFactory>();

		using (ReservoomDbContext dbContext = reservoomDbContextFactory.CreateDbContext()) {
			dbContext.Database.Migrate();
		}

		NavigationService<ReservationListingViewModel> navigationService = _host.Services.GetRequiredService<NavigationService<ReservationListingViewModel>>();
		navigationService.Navigate();

		MainWindow = _host.Services.GetRequiredService<MainWindow>();
		MainWindow.Show();

		base.OnStartup(e);
	}

	protected override void OnExit(ExitEventArgs e) {
		_host.Dispose();

		base.OnExit(e);
	}
}