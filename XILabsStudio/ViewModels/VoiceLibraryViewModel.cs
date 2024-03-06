using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XILabsStudio.API;

namespace XILabsStudio.ViewModels
{
    public partial class VoiceLibraryViewModel
    {
        public VoiceLibraryViewModel() { }

        [RelayCommand]
        private async Task InitializeAsync()
        {
            if (await Shell.Current.DisplayAlert("Not implemented", "This feature is only available through the website user interface.", "Open Website", "Cancel"))
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
        }
    }
}
