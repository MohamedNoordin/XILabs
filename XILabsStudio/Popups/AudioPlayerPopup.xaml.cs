using CommunityToolkit.Maui.Views;
using System.Runtime.Caching;
using CommunityToolkit.Mvvm.Input;
using XILabsStudio.API;
using XILabsStudio.API.DataModels;
using XILabsStudio.Resources.Strings;
using XILabsStudio.ViewModels;
using System.Diagnostics;

namespace XILabsStudio.Popups;

public partial class AudioPlayerPopup : Popup
{
	private Stream speechStream;
	private Voice voice;

	public AudioPlayerPopup(Stream speechStream, Voice voice)
	{
		InitializeComponent();

		this.speechStream = speechStream;
		this.voice = voice;
			BindingContext = new AudioPlayerViewModel(speechStream, voice);
	}

public AudioPlayerPopup(bool continueWhereYouLeftOff)
	{
		InitializeComponent();

        if (continueWhereYouLeftOff)
        {
			BindingContext =
	MemoryCache.Default.Get(CacheKeys.CachedAudioPlayerViewModel) as AudioPlayerViewModel;
        }
	}

	private void Finalize()
	{
        MemoryCache.Default.Set(
    CacheKeys.CachedAudioPlayerViewModel,
    BindingContext as AudioPlayerViewModel,
    MemoryCache.InfiniteAbsoluteExpiration);

		(BindingContext as AudioPlayerViewModel).Dispose();
    }

    private void MinimizeAudioPlayer(object sender, EventArgs e)
    {
		Finalize();
        Close();
    }

    private void Popup_Closed(object sender, CommunityToolkit.Maui.Core.PopupClosedEventArgs e)
    {
		Finalize();
    }
}