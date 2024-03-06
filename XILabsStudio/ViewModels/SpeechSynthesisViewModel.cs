using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using XILabsStudio.API;
using XILabsStudio.API.DataModels;
using XILabsStudio.Popups;
using XILabsStudio.Resources.Strings;

namespace XILabsStudio.ViewModels
{
    [ObservableObject]
    public partial class SpeechSynthesisViewModel : IQueryAttributable
    {
        private XIOpenAPI xi;

        [ObservableProperty]
        private List<Voice> voices;

        [ObservableProperty]
        private Voice selectedVoice;

        [ObservableProperty]
        private VoiceSettings currentVoiceSettings;

        [ObservableProperty]
        private List<Model> models;

        [ObservableProperty]
        private Model selectedModel;

        [ObservableProperty]
        private User user;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private bool isRefreshing;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(InitializeCommand))]
        [NotifyCanExecuteChangedFor(nameof(ReloadCommand))]
        [NotifyCanExecuteChangedFor(nameof(RefreshCommand))]
        [NotifyCanExecuteChangedFor(nameof(GenerateSpeechCommand))]
        [NotifyCanExecuteChangedFor(nameof(ChooseAModelCommand))]
        private bool isConnected;

        public SpeechSynthesisViewModel()
        {
            IsConnected = (Connectivity.Current.NetworkAccess == NetworkAccess.Internet) ? true : false;
            Connectivity.Current.ConnectivityChanged += (s, e) =>
            {
                IsConnected = (Connectivity.Current.NetworkAccess == NetworkAccess.Internet) ? true : false;
                if (IsConnected) InitializeCommand.Execute(null);
            };
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (!IsConnected) return;
            Voices = await xi.GetVoicesAsync();
            SelectedVoice   = Voices.FirstOrDefault(v => v.VoiceID == query["voiceID"] as string);
            query.Clear();
        }

        [RelayCommand(CanExecute = nameof(IsConnected))]
        private async Task InitializeAsync()
        {
            try
            {
                xi = await XIOpenAPI.InitializeAsync();
                User = await xi.GetUserAsync();
                Voices = await xi.GetVoicesAsync();
                if (SelectedVoice is null) SelectedVoice = Voices.FirstOrDefault();
                Models = await xi.GetModelsAsync();
                SelectedModel = Models.FirstOrDefault();
            }
            catch (HttpRequestException ex)
            {
                }
        }

        [RelayCommand(CanExecute = nameof(IsConnected))]
        private async Task ReloadAsync()
        {
            if (xi is null) return;

            User = await xi.GetUserAsync();
            Voices = await xi.GetVoicesAsync();
        }

        [RelayCommand(CanExecute = nameof(IsConnected))]
        private async Task RefreshAsync()
        {
            IsRefreshing = true;
            User = await xi.GetUserAsync();
                                    Voices = await xi.GetVoicesAsync();
            if (SelectedVoice is null) SelectedVoice = Voices.FirstOrDefault();
            Models = await xi.GetModelsAsync();
            SelectedModel = Models.FirstOrDefault();
            IsRefreshing = false;
        }

        [RelayCommand(CanExecute = nameof(CanGenerateSpeech))]
        private async Task GenerateSpeech(string? text)
        {
            IsBusy = true;
                                        var speechStream = await xi.TextToSpeech(
                                        new API.DataModels.TextToSpeech
                                        {
                                            Text = text,
                                            ModelID = SelectedModel.ModelID,
                                            VoiceSettings = CurrentVoiceSettings
                                        },
                                        SelectedVoice.VoiceID
                                        );
            User.Subscription.CharacterCount += text.Length;
            IsBusy = false;

            await Shell.Current.ShowPopupAsync(new AudioPlayerPopup(speechStream, SelectedVoice));

            var maximizeAudioPlayerToolbarItem =
            new ToolbarItem
            {
                Text = XIResources.AudioPlayerPopup_MaximizeAudioPlayer,
                IconImageSource = "maximize",
                Command = MaximizeAudioPlayerCommand
            };

            if (!Shell.Current.ToolbarItems.Any(i => i.Text == XIResources.AudioPlayerPopup_MaximizeAudioPlayer))
                Shell.Current.ToolbarItems.Add(maximizeAudioPlayerToolbarItem);
        }

        private bool CanGenerateSpeech(string? text)
        {
            return IsConnected && !string.IsNullOrWhiteSpace(text) &&
                   User != null && User.Subscription != null &&
                   User.Subscription.CharacterCount != User.Subscription.CharacterLimit &&
                   !IsBusy;
        }

        [RelayCommand]
        private async Task AddVoice()
        {
            await Shell.Current.GoToAsync("//VoiceLabPage", true);
        }

        [RelayCommand]
        private async Task OpenVoiceSettings()
        {
            var result = await Shell.Current.ShowPopupAsync(new VoiceSettingsPopup(CurrentVoiceSettings));
            if (result is API.DataModels.VoiceSettings voiceSettingsResult) CurrentVoiceSettings = voiceSettingsResult;
        }

        [RelayCommand(CanExecute = nameof(IsConnected))]
        private async Task ChooseAModel()
        {
            var result = await Shell.Current.ShowPopupAsync(new ChooseAModelPopup());
            if (result is Model modelResult) SelectedModel = modelResult;
        }

        [RelayCommand]
        private async Task MaximizeAudioPlayerAsync() =>
            await Shell.Current.ShowPopupAsync(new AudioPlayerPopup(true));
    }
}
