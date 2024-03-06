using CommunityToolkit.Maui.Views;
using XILabsStudio.Resources.Strings;

namespace XILabsStudio.Popups;

public partial class AddVoicePopup : Popup
{
	public AddVoicePopup()
	{
		InitializeComponent();

//		this.Window.Title = XIResources.AddVoicePopup_TypeOfVoiceToCreate;
		this.Opened += (s, e) => FocusHolder.Focus();
	}
}