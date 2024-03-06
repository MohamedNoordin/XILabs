using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XILabsStudio.API.DataModels
{
    [ObservableObject]
    public partial class Subscription
    {
        [JsonProperty("tier")]
        public string Tier { get; set; }

        [JsonProperty("character_count")]
        [ObservableProperty]
        private int characterCount;

        [JsonProperty("character_limit")]
        public int CharacterLimit { get; set; }

        public string RemainingQuota
        {
            get { return (CharacterLimit - CharacterCount).ToString(); }
        }

        [JsonProperty("can_extend_character_limit")]
        public bool CanExtendCharacterLimit { get; set; }

        [JsonProperty("allowed_to_extend_character_limit")]
        public bool AllowedToExtendCharacterLimit { get; set; }

        [JsonProperty("next_character_count_reset_unix")]
        public int NextCharacterCountResetUnix { get; set; }

        [JsonProperty("voice_limit")]
        public int VoiceLimit { get; set; }

        [JsonProperty("max_voice_add_edits")]
        public int MaxVoiceAddEdits { get; set; }

        [JsonProperty("voice_add_edit_counter")]
        public int VoiceAddEditCounter { get; set; }

        [JsonProperty("professional_voice_limit")]
        public int ProfessionalVoiceLimit { get; set; }

        [JsonProperty("can_extend_voice_limit")]
        public bool CanExtendVoiceLimit { get; set; }

        [JsonProperty("can_use_instant_voice_cloning")]
        public bool CanUseInstantVoiceCloning { get; set; }

        [JsonProperty("can_use_professional_voice_cloning")]
        public bool CanUseProfessionalVoiceCloning { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    [ObservableObject]
    public partial class User
    {
        [JsonProperty("subscription")]
        [ObservableProperty]
        private Subscription subscription;

        [JsonProperty("is_new_user")]
        public bool IsNewUser { get; set; }

        [JsonProperty("xi_api_key")]
        public string XiApiKey { get; set; }

        [JsonProperty("can_use_delayed_payment_methods")]
        public bool CanUseDelayedPaymentMethods { get; set; }
    }
}
