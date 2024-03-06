using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XILabsStudio.API;

namespace XILabsStudio.ViewModels
{
    public partial class ProjectsViewModel
    {
        public ProjectsViewModel() { }

        [RelayCommand]
        private async Task InitializeAsync()
        {
            if (await Shell.Current.DisplayAlert("Not implemented", "This feature is only available through the website user interface.", "Open Website", "Cancel"))
            {
                try
                {
                    var projectsUri = new Uri(Endpoints.Website.Projects);
                    await Browser.Default.OpenAsync(projectsUri, BrowserLaunchMode.SystemPreferred);
                }
                catch (Exception)
                {
                    // An unexpected error occurred. No browser may be installed on the device.
                }
            }
        }
    }
}
