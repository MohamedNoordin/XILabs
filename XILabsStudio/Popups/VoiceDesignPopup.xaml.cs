using CommunityToolkit.Maui.Views;

namespace XILabsStudio.Popups;

public partial class VoiceDesignPopup : Popup
{
	public VoiceDesignPopup()
	{
		InitializeComponent();
		GetFocus.Focus();
	}
}