using MVCStoreClient.Services;
using MVCStoreClient.ViewModels;
using MVCStoreClient.Views;

namespace MVCStoreClient;

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
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddScoped<IClientService, ClientService>();

        builder.Services.AddTransient<HomePageViewModel>();
        builder.Services.AddTransient<HomePage>();

        return builder.Build();
    }
}
