using AITinker.Core.Models;
using AITinker.OpenAI.Services;
using AITinker.ViewModels;

using Microsoft.Extensions.Configuration;

namespace AITinker;

public partial class App : Application {
    private const string _title = "AI Tinker";

    public static IServiceProvider? ServiceProvider { get; private set; }

    public App(IServiceProvider serviceProvider) {
        InitializeComponent();
        ServiceProvider = serviceProvider;

        var openAIService = serviceProvider.GetRequiredService<OpenAIService>();
        var chatViewModel = serviceProvider.GetRequiredService<ChatViewModel>();
        var configurations = serviceProvider.GetRequiredService<Configurations>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        chatViewModel.SetServices(configuration, configurations, openAIService);

        MainPage = new Views.ChatPage() { BindingContext = chatViewModel };
    }

    protected override Window CreateWindow(IActivationState? activationState) {
        var window = base.CreateWindow(activationState);
        if (DeviceInfo.Current.Platform == DevicePlatform.WinUI) {
            window.Title = _title;
        }

        return window;
    }
}
