using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Resources;
using XILabsStudio.API;
using XILabsStudio.Resources.Strings;

namespace XILabsStudio.Pages;

public partial class FirstRunExperiencePage : ContentPage
{
	public FirstRunExperiencePage()
	{
		InitializeComponent();
        BindingContext = this;
	}

    [RelayCommand]
    private async Task ConnectAsync(object xiAPIKey)
    {
        if ((xiAPIKey as string) == null)
        {
            await App.Current.MainPage.DisplayAlert(XIResources.FirstRunXP_EmptyAPIKeyAlertTitle, XIResources.FirstRunXP_EmptyAPIKeyAlertDescription, "OK");
            return;
        }

        if (await XIOpenAPI.ConnectAsync((xiAPIKey as string)))
        {
#if WINDOWS || MACCATALYST
            App.Current.MainPage = new DesktopAppShell();
#elif ANDROID || IOS
			App.Current.MainPage = new MobileAppShell();
#endif
        }
        else
            await App.Current.MainPage.DisplayAlert(XIResources.FirstRunXP_WrongAPIKeyAlertTitle, $"{XIResources.FirstRunXP_WrongAPIKeyAlertDescription} {Endpoints.Website.ElevenLabs}", "OK");
    }
}