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
    public partial class VoiceDetailsViewModel
    {
        private XIOpenAPI xi;

        [ObservableProperty]
        private User user;

        [ObservableProperty]
        private string pageTitle;

        [ObservableProperty]
        private string mainAction;

        [ObservableProperty]
        private string labelsCount;

        [ObservableProperty]
        private string addedLabelsCount;

        [ObservableProperty]
        private string labels;

        [ObservableProperty]
        private object mainObject;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(MainActionCommand))]
        private string voiceName;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(MainActionCommand))]
        private string voiceDescription;

        [ObservableProperty]
        private string gender;

        [ObservableProperty]
        private string age;

        [ObservableProperty]
        private string accent;

        public VoiceDetailsViewModel(object mainObject)
        {
            this.MainObject = mainObject;

            if (MainObject is VoiceDesign voiceDesign)
            {
                PageTitle = XIResources.VoiceDesignPopup_GenerateVoice;
                MainAction = PageTitle;

                VoiceName = voiceDesign.Name;
                VoiceDescription = voiceDesign.VoiceDescription;
            Accent = System.Enum.GetName(voiceDesign.Accent);
            Age = System.Enum.GetName(voiceDesign.Age);
            Gender = System.Enum.GetName(voiceDesign.Gender);
            }
            else if (MainObject is Voice voice)
            {
                PageTitle = XIResources.EditVoice;
                MainAction = PageTitle;

                VoiceName = voice.Name;
                VoiceDescription= voice.Description;
                Accent = voice.Labels.AdditionalProp1;
                Age = voice.Labels.AdditionalProp2;
                Gender = voice.Labels.AdditionalProp3;
            }

        }

        [RelayCommand]
        private async Task InitializeAsync()
        {
            xi = await XIOpenAPI.InitializeAsync();
            User = await xi.GetUserAsync();
        }

        [RelayCommand(CanExecute = nameof(CanDoMainAction))]
       private async Task MainActionAsync(object popup)
        {
            if (MainObject is VoiceDesign voiceDesign)
            {
                voiceDesign.Name = VoiceName;
                voiceDesign.VoiceDescription = VoiceDescription;
                await xi.CreateVoiceAsync(voiceDesign);
            }
            else if (MainObject is Voice voice)
            {
                voice.Name = VoiceName;
                voice.Description = VoiceDescription;
                await xi.EditVoiceAsync(voice);
            }

            if (popup is Popup) await (popup as Popup).CloseAsync();
        }

        private bool CanDoMainAction()
        {
            if (MainObject is VoiceDesign voiceDesign)
            {
                // VoiceDesign specific checks
                if (this.VoiceName != voiceDesign.Name &&
                    !string.IsNullOrWhiteSpace(this.VoiceName))
                {
                    return true;
                }

                if (this.VoiceDescription != voiceDesign.VoiceDescription)
                {
                    return true;
                }
            }
            else if (MainObject is Voice voice)
            {
                // Voice specific checks
                if (this.VoiceName != voice.Name &&
                    !string.IsNullOrWhiteSpace(this.VoiceName))
                {
                    return true;
                }

                if (this.VoiceDescription != voice.Description)
                {
                    return true;
                }
            }

            return false;
        }

        [RelayCommand]
        private async Task CancelAsync(object popup)
        {
            if (popup is Popup) await (popup as Popup).CloseAsync();
        }

    }
}
