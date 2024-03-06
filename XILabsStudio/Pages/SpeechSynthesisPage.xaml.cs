using CommunityToolkit.Maui.Views;
using XILabsStudio.Popups;
using XILabsStudio.Resources.Strings;

namespace XILabsStudio.Pages;

public partial class SpeechSynthesisPage : ContentPage
{
	public SpeechSynthesisPage()
	{
		InitializeComponent();

        Appearing += TTSInputEditorSetFocus;
    }

    private void TTSInputEditorSetFocus(object sender, EventArgs e)
    {
        TTSInputEditor.Focus();
    }
}

