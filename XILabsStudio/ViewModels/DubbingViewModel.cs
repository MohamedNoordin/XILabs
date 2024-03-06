using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XILabsStudio.API;

namespace XILabsStudio.ViewModels
{
    public partial class DubbingViewModel
    {
        public DubbingViewModel() { }

        [RelayCommand]
        private async Task InitializeAsync()
        {
            if (await Shell.Current.DisplayAlert("Not implemented", "This feature is only available through the website user interface.", "Open Website", "Cancel"))
            {
                try
                {
                    var dubbingUri = new Uri(Endpoints.Website.Dubbing);
                    await Browser.Default.OpenAsync(dubbingUri, BrowserLaunchMode.SystemPreferred);
                }
                catch (Exception)
                {
                    // An unexpected error occurred. No browser may be installed on the device.
                }
            }
        }
    }
}
