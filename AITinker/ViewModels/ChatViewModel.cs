﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using AITinker.Models;

namespace AITinker.ViewModels;

internal class ChatViewModel : INotifyPropertyChanged {
    private OpenAI.Services.OpenAIService? _openAIService;
    private string? _userMessage;
    private bool _isEditingApiKey;

    public ObservableCollection<MessageEntry> MessageEntries { get; private set; }

    public string UserMessage {
        get => _userMessage ?? string.Empty;
        set {
            _userMessage = value;
            OnPropertyChanged();
        }
    }

    public string ApiKey {
        get {
            return _openAIService?.ApiKey ?? string.Empty;
        }
        set {
            if (_openAIService != null) {
                _openAIService.ApiKey = value;
                OnPropertyChanged();
            }
        }
    }

    public string ApiKeyDisplay {
        get {
            if (_isEditingApiKey)
                return ApiKey;  // Show full key while editing

            if (string.IsNullOrEmpty(ApiKey))
                return string.Empty;

            if (ApiKey.Length <= 12)
                return ApiKey; // Do not obscure if the key is too short

            return $"{ApiKey[..6]}...{ApiKey[^6..]}";
        }
    }

    public string ApiUrl {
        get {
            return _openAIService?.ApiUrl ?? string.Empty;
        }
        set {
            if (_openAIService != null) {
                _openAIService.ApiUrl = value;
                OnPropertyChanged();
            }
        }
    }

    public string Model {
        get {
            return _openAIService?.Model ?? string.Empty;
        }
        set {
            if (_openAIService != null) {
                _openAIService.Model = value;
                OnPropertyChanged();
            }
        }
    }

    public string SystemContent {
        get {
            return _openAIService?.SystemContent ?? string.Empty;
        }
        set {
            if (_openAIService != null) {
                _openAIService.SystemContent = value;
                OnPropertyChanged();
            }
        }
    }

    public double Temperature {
        get {
            return _openAIService?.Temperature ?? 0.5;
        }
        set {
            if (_openAIService != null) {
                _openAIService.Temperature = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsEditingApiKey {
        get => _isEditingApiKey;
        set {
            _isEditingApiKey = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ApiKeyDisplay));
        }
    }

    public ICommand SendMessageCommand { get; }

    public ICommand StartEditApiKeyCommand { get; }

    public ICommand FinishEditApiKeyCommand { get; }


    public ChatViewModel() {
        SendMessageCommand = new Command(async () => await SendMessage());
        StartEditApiKeyCommand = new Command(() => IsEditingApiKey = true);
        FinishEditApiKeyCommand = new Command(() => IsEditingApiKey = false);
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

