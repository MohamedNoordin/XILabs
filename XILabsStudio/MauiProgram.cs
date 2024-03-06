using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace XILabsStudio;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
#if IOS
        AVAudioSession.SharedInstance().SetActive(true);
        AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Playback);
#endif
        var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            // Initialize the .NET MAUI Community Toolkit by adding the below line of code
            .UseMauiCommunityToolkit()
            // Initialize the .NET MAUI Community Toolkit MediaElement by adding the below line of code
            .UseMauiCommunityToolkitMediaElement()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("AtkinsonHyperlegible-Regular.ttf", "AtkinsonHyperlegible");
				fonts.AddFont("Inter-Regular.ttf", "Inter");
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
