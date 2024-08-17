using AITinker.ViewModels;

namespace AITinker.Views;

public partial class ChatPage : ContentPage {
    public ChatPage() {
        InitializeComponent();
        var viewModel = BindingContext as ChatViewModel;

        if (viewModel is not null) {
            viewModel.PromptSent += ViewModel_PromptSent;
        }
    }

    private void ViewModel_PromptSent(object? sender, EventArgs e) {
        PromptEditor.Focus();
    }

    private void OnEntryCompleted(object sender, EventArgs e) {
        var viewModel = BindingContext as ChatViewModel;

        if (viewModel != null && viewModel.SendMessageCommand.CanExecute(null)) {
            viewModel.SendMessageCommand.Execute(null);
        }
    }
}

