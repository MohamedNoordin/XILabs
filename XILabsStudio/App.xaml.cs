using XILabsStudio.API;
using XILabsStudio.Pages;
using XILabsStudio.Resources.Strings;

namespace XILabsStudio;

public partial class App : Application
{
	public App()
	{
		// Initialize VersionTracking
		VersionTracking.Track();

		InitializeComponent();

		if (!XIOpenAPI.IsInitialized())
		{
			// First-Run Experience
			MainPage = new FirstRunExperiencePage();
		}
		else
		{
#if WINDOWS || MACCATALYST
			MainPage = new DesktopAppShell();
#elif ANDROID || IOS
			MainPage = new MobileAppShell();
#endif
		}
	}

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);
		if (window != null)
			window.Title = "XILabsStudio";

		return window;
    }
}
