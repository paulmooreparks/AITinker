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
}
