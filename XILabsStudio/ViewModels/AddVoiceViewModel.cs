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
    public partial class AddVoiceViewModel
    {
        private XIOpenAPI xi;

        [ObservableProperty]
        private User user;

        public AddVoiceViewModel()
        {
        }

        [RelayCommand]
        private async Task InitializeAsync()
        {
            xi = await XIOpenAPI.InitializeAsync();
            User = await xi.GetUserAsync();
        }

        [RelayCommand]
        private async Task GoToVoiceDesignAsync(object popup)
        {
            if (popup is Popup) await (popup as Popup).CloseAsync();
            await Shell.Current.ShowPopupAsync(new VoiceDesignPopup());
        }

        [RelayCommand]
        private async Task GoToSubscriptionAsync()
        {
            try
            {
                var subscriptionUri = new Uri(Endpoints.Website.Subscription);
                await Browser.Default.OpenAsync(subscriptionUri, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception)
            {
                // An unexpected error occurred. No browser may be installed on the device.
            }
        }

        [RelayCommand]
        private async Task GoToInstantVoiceCloningAsync()
        {
            if (await Shell.Current.DisplayAlert("Not implemented", "This feature is only available through the website user interface.", "Open Website", "Cancel"))
            {
                try
                {
                    var voiceLabUri = new Uri(Endpoints.Website.VoiceLab);
                    await Browser.Default.OpenAsync(voiceLabUri, BrowserLaunchMode.SystemPreferred);
                }
                catch (Exception)
                {
                    // An unexpected error occurred. No browser may be installed on the device.
                }
            }
        }

        private bool CanUseInstantVoiceCloning() => User != null ? User.Subscription.CanUseInstantVoiceCloning : false;

        [RelayCommand]
        private async Task GoToVoiceLibraryAsync()
        {
            try
            {
                var voiceLibraryUri = new Uri(Endpoints.Website.VoiceLibrary);
                await Browser.Default.OpenAsync(voiceLibraryUri, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception)
            {
                // An unexpected error occurred. No browser may be installed on the device.
            }
        }

        [RelayCommand]
        private async Task GoToProfessionalVoiceCloningAsync()
        {
            if (await Shell.Current.DisplayAlert("Not implemented", "This feature is only available through the website user interface.", "Open Website", "Cancel"))
            {
                try
                {
                    var voiceLabUri = new Uri(Endpoints.Website.VoiceLab);
                    await Browser.Default.OpenAsync(voiceLabUri, BrowserLaunchMode.SystemPreferred);
                }
                catch (Exception)
                {
                    // An unexpected error occurred. No browser may be installed on the device.
                }
            }
        }

        private bool CanUseProfessionalVoiceCloning() => User != null ? User.Subscription.CanUseProfessionalVoiceCloning : false;

    }
}
