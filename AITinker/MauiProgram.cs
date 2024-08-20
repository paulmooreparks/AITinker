using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

using AITinker.OpenAI.Extensions;
using AITinker.ViewModels;
using Microsoft.Maui.LifecycleEvents;
using AITinker.Core.Extensions;


namespace AITinker;
public static class MauiProgram {
    static MauiProgram() {

    }
    public static MauiApp CreateMauiApp() {
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.SetFileProvider(AITinker.Core.Configuration.FileProvider);
        configurationBuilder.AddJsonFile(AITinker.Core.Configuration.ConfigFileName, optional: true, reloadOnChange: true);
        var configuration = configurationBuilder.Build();

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureLifecycleEvents(events => {
#if WINDOWS
                events.AddWindows(windows =>
                {
                    windows.OnWindowCreated(window =>
                    {
                        window.ExtendsContentIntoTitleBar = false;
                        window.Title = "AI Tinker";
                    });
                });
#endif            
            });

        builder.Services.AddSingleton<IConfiguration>(configuration);
        builder.Services.AddSingleton<IFileProvider>(AITinker.Core.Configuration.FileProvider);
        builder.Services.AddLLMServices(configuration);
        builder.Services.AddOpenAIServices(configuration);
        builder.Services.AddSingleton<OpenAI.Services.OpenAIService>();
        builder.Services.AddSingleton<ChatViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
