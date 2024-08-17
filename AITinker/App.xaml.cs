using AITinker.OpenAI.Services;
using AITinker.ViewModels;

namespace AITinker;

public partial class App : Application {
    public static IServiceProvider? ServiceProvider { get; private set; }

    public App(IServiceProvider serviceProvider) {
        InitializeComponent();
        ServiceProvider = serviceProvider;

        var openAIService = serviceProvider.GetRequiredService<OpenAIService>();
        var chatViewModel = serviceProvider.GetRequiredService<ChatViewModel>();
        chatViewModel.SetOpenAIService(openAIService);

        MainPage = new Views.ChatPage() { BindingContext = chatViewModel };
    }

    protected override Window CreateWindow(IActivationState? activationState) {
        var window = base.CreateWindow(activationState);
        if (DeviceInfo.Current.Platform == DevicePlatform.WinUI) {
            window.Title = "AI Tinker"; // System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name;
        }

        return window;
    }
}
