using Microsoft.Extensions.Logging;

namespace MiPrimeraAplicacion
{
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

            // Configurar imágenes para que se muestren correctamente
            builder.ConfigureMauiHandlers(handlers =>
            {
                Microsoft.Maui.Handlers.ImageHandler.Mapper.AppendToMapping("Fondo", (handler, view) =>
                {
                    if (view is Image image)
                    {
                        image.Aspect = Aspect.AspectFill; // Asegura que la imagen cubra el fondo correctamente
                    }
                });
            });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
