using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.Audio;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XILabsStudio.API;
using XILabsStudio.API.DataModels;
using XILabsStudio.Popups;
using XILabsStudio.Resources.Strings;

namespace XILabsStudio.ViewModels
{
    [ObservableObject]
    public partial class VoiceLabViewModel
    {
        private XIOpenAPI xi;

        [ObservableProperty]
        private User user;

        [ObservableProperty]
        private ObservableCollection<Voice> voices;

        [ObservableProperty]
        private bool isRefreshing;

        public VoiceLabViewModel()
        {
        }

        [RelayCommand]
        private async Task InitializeAsync()
        {
            xi = await XIOpenAPI.InitializeAsync();
            User = await xi.GetUserAsync();

            var unfilteredVoices = await xi.GetVoicesAsync();
            Voices = unfilteredVoices.Where(v => v.Category != "premade").ToObservableCollection();
            AddVoiceCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            IsRefreshing = true;
            var unfilteredVoices = await xi.GetVoicesAsync();
            Voices = unfilteredVoices.Where(v => v.Category != "premade").ToObservableCollection();
            AddVoiceCommand.NotifyCanExecuteChanged();
            IsRefreshing = false;
        }

        [RelayCommand]
        private async Task UseVoiceAsync(object voice)
        {
            await Shell.Current.GoToAsync("///SpeechSynthesisPage", true,
                                new Dictionary<string, object>
                {
                    { "voiceID", (voice as Voice).VoiceID }
                }
                );
        }

        [RelayCommand]
        private async Task UseThisAsync(object sender)
        {
            await Shell.Current.GoToAsync("//SpeechSynthesisPage", true,
                new Dictionary<string, object>
                {
                    { "voiceID", (sender as Voice).VoiceID }
                }
                );
        }

        [RelayCommand]
        private async Task EditVoiceAsync(object voice)
        {
            await Shell.Current.ShowPopupAsync(new VoiceDetailsPopup(voice));
        }

        [RelayCommand]
        private async Task EditThisAsync(object voice)
        {
            await Shell.Current.ShowPopupAsync(new VoiceDetailsPopup(voice));
        }

        [RelayCommand]
        private async Task RemoveVoiceAsync(object voice)
        {
            await xi.DeleteVoiceAsync((voice as Voice).VoiceID);
            Voices.Remove((voice as Voice));
            AddVoiceCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand]
        private async Task RemoveThisAsync(object sender)
        {
            await xi.DeleteVoiceAsync((sender as Voice).VoiceID);
            Voices.Remove((sender as Voice));
            AddVoiceCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanAddVoices))]
        private async Task AddVoiceAsync()
        {
            await Shell.Current.ShowPopupAsync(new Popups.AddVoicePopup());
        }

        private bool CanAddVoices() => Voices != null ? Voices.Count < 3 : true;
    }
}
