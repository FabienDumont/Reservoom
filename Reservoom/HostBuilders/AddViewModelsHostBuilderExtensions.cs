using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVVMEssentials.Services;
using MVVMEssentials.Stores;
using Reservoom.Stores;
using Reservoom.ViewModels;

namespace Reservoom.HostBuilders;

public static class AddViewModelsHostBuilderExtensions {
	public static IHostBuilder AddViewModels(this IHostBuilder hostBuilder) {
		hostBuilder.ConfigureServices(
			services => {
				services.AddTransient(CreateReservationListingViewModel);
				services.AddSingleton(
					s => new NavigationService<ReservationListingViewModel>(
						s.GetRequiredService<NavigationStore>(), s.GetRequiredService<ReservationListingViewModel>
					)
				);

				services.AddTransient<MakeReservationViewModel>(
					s => new MakeReservationViewModel(
						s.GetRequiredService<HotelStore>(), s.GetRequiredService<NavigationService<ReservationListingViewModel>>()
					)
				);
				services.AddSingleton(
					s => new NavigationService<MakeReservationViewModel>(
						s.GetRequiredService<NavigationStore>(), s.GetRequiredService<MakeReservationViewModel>
					)
				);
			}
		);

		return hostBuilder;
	}
	
	private static ReservationListingViewModel CreateReservationListingViewModel(IServiceProvider services) {
		return ReservationListingViewModel.LoadViewModel(
			services.GetRequiredService<HotelStore>(), services.GetRequiredService<NavigationService<MakeReservationViewModel>>()
		);
	}
}