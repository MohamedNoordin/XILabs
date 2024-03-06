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
    public partial class VoiceDesignViewModel
    {
        private XIOpenAPI xi;

        [ObservableProperty]
        private VoiceDesign voiceDesign;

        [ObservableProperty]
        private User user;

        [ObservableProperty]
        private List<string> genders;

        [ObservableProperty]
        private Gender selectedGender;

        [ObservableProperty]
        private List<string> ages;

        [ObservableProperty]
        private Age selectedAge;

        [ObservableProperty]
        private List<string> accents;

        [ObservableProperty]
        private Accent selectedAccent;

        [ObservableProperty]
        private string selectedAccentStrength;

        [ObservableProperty]
        private bool isBusy;

        public VoiceDesignViewModel()
        {
        }

        [RelayCommand]
        private async Task InitializeAsync()
        {
            xi = await XIOpenAPI.InitializeAsync();
            User = await xi.GetUserAsync();

            Genders = System.Enum.GetNames(typeof(Gender)).ToList();
            Ages = System.Enum.GetNames(typeof(Age)).Select(a => a.SplitCamelCase()).ToList();
            Accents = System.Enum.GetNames(typeof(Accent)).ToList();
        }

        [RelayCommand(CanExecute = nameof(CanGenerateVoice))]
        private async Task GenerateVoiceAsync(string? text)
        {
            IsBusy = true;
            VoiceDesign = await xi.GenerateVoiceAsync(
            new API.DataModels.VoiceDesign
            {
                Name = "Test",
                Text = text,
                VoiceDescription = "Test",
                Gender = SelectedGender,
                Age = SelectedAge,
                Accent = SelectedAccent,
                AccentStrength = (SelectedAccentStrength == "Low" ? 0.3f : SelectedAccentStrength == "Medium" ? 1.5f : SelectedAccentStrength == "High" ? 2.0f : 1.5f)
            }
            );
            User.Subscription.CharacterCount += text.Length;

            UseVoiceCommand.NotifyCanExecuteChanged();
            IsBusy = false;

            await Shell.Current.ShowPopupAsync(new AudioPlayerPopup(voiceDesign.Audio, new Voice { Name = "Generated Voice" }));

            var maximizeAudioPlayerToolbarItem =
            new ToolbarItem
            {
                Text = XIResources.AudioPlayerPopup_MaximizeAudioPlayer,
                Command = MaximizeAudioPlayerCommand
            };

            if (!Shell.Current.ToolbarItems.Any(i => i.Text == XIResources.AudioPlayerPopup_MaximizeAudioPlayer))
                Shell.Current.ToolbarItems.Add(maximizeAudioPlayerToolbarItem);
        }

        private bool CanGenerateVoice(string? text) => true;
//            (!string.IsNullOrWhiteSpace(text)) && (User.Subscription.CharacterCount != User.Subscription.CharacterLimit) && !IsBusy;

        [RelayCommand(CanExecute = nameof(CanUseVoice))]
        private async Task UseVoiceAsync()
        {
            await Shell.Current.ShowPopupAsync(new VoiceDetailsPopup(VoiceDesign));
        }

        private bool CanUseVoice() =>
            VoiceDesign != null;

        [RelayCommand]
        private async Task CloseAsync(object popup)
        {
            if (popup is Popup) await (popup as Popup).CloseAsync();
        }

        [RelayCommand]
        private async Task MaximizeAudioPlayerAsync() =>
    await Shell.Current.ShowPopupAsync(new AudioPlayerPopup(true));
}
}
