using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using AITinker.Models;

namespace AITinker.ViewModels;

internal class ChatViewModel : INotifyPropertyChanged {
    private OpenAI.Services.OpenAIService? _openAIService;
    private string? _userMessage;

    public ObservableCollection<MessageEntry> MessageEntries { get; private set; }

    public string UserMessage {
        get => _userMessage ?? string.Empty;
        set {
            _userMessage = value;
            OnPropertyChanged();
        }
    }

    public ICommand SendMessageCommand { get; }

    public ChatViewModel() {
        SendMessageCommand = new Command(async () => await SendMessage());
        MessageEntries = new ObservableCollection<MessageEntry>();
    }

    public void SetOpenAIService(OpenAI.Services.OpenAIService openAIService) {
        _openAIService = openAIService;
    }

    public event EventHandler? PromptSent;

    private async Task SendMessage() {
        if (_openAIService == null) {
            throw new InvalidOperationException("OpenAIService is not initialized.");
        }

        if (!string.IsNullOrEmpty(UserMessage)) {

            MessageEntries.Add(new MessageEntry {
                Message = UserMessage,
                Source = MessageSource.User
            });

            var submissionText = UserMessage;
            UserMessage = string.Empty; 

            var response = await _openAIService.SendMessage(submissionText, string.Empty);

            MessageEntries.Add(new MessageEntry {
                Message = response.Content,
                Source = MessageSource.LLM
            });
        }

        PromptSent?.Invoke(this, new EventArgs());
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
