using CommunityToolkit.Maui.Views;
using XILabsStudio.API.DataModels;

namespace XILabsStudio.Popups;

public partial class VoiceDetailsPopup : Popup
{
	public VoiceDetailsPopup(object obj)
	{
		InitializeComponent();
		BindingContext = new ViewModels.VoiceDetailsViewModel(obj);
	}
}