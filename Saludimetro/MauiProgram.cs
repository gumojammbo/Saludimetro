using Microsoft.Extensions.Logging;
using Saludimetro.DataAccess;
using Saludimetro.ViewModels;
using Saludimetro.Views;

namespace Saludimetro;

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

        var dbContext = new PatientDbContext();
        dbContext.Database.EnsureCreated();
        dbContext.Dispose();

        builder.Services.AddDbContext<PatientDbContext>();

        // Registrar páginas y ViewModels
        builder.Services.AddTransient<PatientPage>();
        builder.Services.AddTransient<PatientViewModel>();

        builder.Services.AddTransient<MainPage>();

        // Añadir estos registros
        builder.Services.AddTransient<PatientListPage>();
        builder.Services.AddTransient<PatientListViewModel>();

        Routing.RegisterRoute(nameof(PatientPage), typeof(PatientPage));
        Routing.RegisterRoute(nameof(PatientListPage), typeof(PatientListPage)); // Asegúrate de registrar la ruta


#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
