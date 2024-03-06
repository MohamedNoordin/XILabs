
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using XILabsStudio.API.DataModels;
using XILabsStudio.Messages;

namespace XILabsStudio.Pages;

public partial class HistoryPage : ContentPage
{
	public HistoryPage()
	{
		InitializeComponent();
#if ANDROID || IOS
		XILabsStudio.Services.DeviceOrientationService.SetOrientation(XILabsStudio.Services.DeviceOrientation.Landscape);
#endif

		WeakReferenceMessenger.Default.Register<HistoryChangedMessage>(this, (s, e) =>
		{
			Debug.WriteLine((e.Value as History).AllAbout);
			HistoryListView.ScrollTo((e.Value as History), ScrollToPosition.Start, false);
			}
			);
		}
}