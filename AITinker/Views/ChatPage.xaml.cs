using AITinker.ViewModels;

namespace AITinker.Views;

public partial class ChatPage : ContentPage {
    public ChatPage() {
        InitializeComponent();
    }

    private void OnEntryCompleted(object sender, EventArgs e) {
        var viewModel = BindingContext as ChatViewModel;
        if (viewModel != null && viewModel.SendMessageCommand.CanExecute(null)) {
            viewModel.SendMessageCommand.Execute(null);
        }
    }
}

