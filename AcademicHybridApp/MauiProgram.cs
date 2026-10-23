using AcademicHybridApp.Services;
using Microsoft.Extensions.Logging;

namespace AcademicHybridApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();

		builder.Services.AddSingleton(_ => new HttpClient
		{
			BaseAddress = new Uri(ApiConfig.BaseUrl),
			Timeout = TimeSpan.FromSeconds(15)
		});
		builder.Services.AddSingleton<DosenService>();
		builder.Services.AddSingleton<MahasiswaService>();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
