using CommunityToolkit.Maui.Views;
using XILabsStudio.API.DataModels;

namespace XILabsStudio.Popups;

public partial class VoiceSettingsPopup : Popup
{
	private VoiceSettings settings;

	public VoiceSettingsPopup(VoiceSettings voiceSettings)
	{
		InitializeComponent();

		if (voiceSettings is not null)
			settings = voiceSettings;
		else settings = new VoiceSettings();

        stabilitySlider.Value = settings.Stability;
        clarityPlusSimilarityEnhancementSlider.Value = settings.SimilarityBoost;
        styleSlider.Value = settings.Style;

        speakerBoostCheckBox.IsChecked = settings.UseSpeakerBoost;

        ResultWhenUserTapsOutsideOfPopup = settings;
    }

    private void ApplySettings(object sender, EventArgs e)
    {
		settings.Stability = stabilitySlider.Value;
		settings.SimilarityBoost = clarityPlusSimilarityEnhancementSlider.Value;
		settings.Style = styleSlider.Value;

		settings.UseSpeakerBoost = speakerBoostCheckBox.IsChecked;

		ResultWhenUserTapsOutsideOfPopup = settings;
    }

	private void ToDefault(object sender, EventArgs e)
	{
		settings = new VoiceSettings();
		stabilitySlider.Value = settings.Stability;
		clarityPlusSimilarityEnhancementSlider.Value = settings.SimilarityBoost;
		styleSlider.Value = settings.Style;

		speakerBoostCheckBox.IsChecked = settings.UseSpeakerBoost;

		ResultWhenUserTapsOutsideOfPopup = settings;
	}
}