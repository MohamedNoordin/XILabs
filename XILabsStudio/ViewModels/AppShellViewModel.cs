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
    public partial class AppShellViewModel
    {
        [ObservableProperty]
        private bool isConnected;

        public AppShellViewModel()
        {
        }

        [RelayCommand]
        private async Task InitializeAsync()
        {
            IsConnected = (Connectivity.Current.NetworkAccess == NetworkAccess.Internet) ? true : false;
            Connectivity.Current.ConnectivityChanged += (s, e) =>
            {
                IsConnected = (Connectivity.Current.NetworkAccess == NetworkAccess.Internet) ? true : false;
            };
        }

        [RelayCommand]
        private async Task SignOutAsync()
        {
            SecureStorage.Default.RemoveAll();
            await Toast.Make("Signed out.").Show();
            App.Current.MainPage = new Pages.FirstRunExperiencePage();
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
        private async Task GoToUsageAnalyticsAsync()
        {
            try
            {
                var usageUri = new Uri(Endpoints.Website.Usage);
                await Browser.Default.OpenAsync(usageUri, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception)
            {
                // An unexpected error occurred. No browser may be installed on the device.
            }
        }

    }
}
