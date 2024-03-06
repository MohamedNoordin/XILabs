using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XILabsStudio.API.DataModels
{
    public class ModelsResponse
    {
        public List<Model> Models { get; set; }
    }

    public class Language
    {
        [JsonProperty("language_id")]
        public string LanguageID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Model
    {
        [JsonProperty("model_id")]
        public string ModelID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("can_be_finetuned")]
        public bool CanBeFinetuned { get; set; }

        [JsonProperty("can_do_text_to_speech")]
        public bool CanDoTextToSpeech { get; set; }

        [JsonProperty("can_do_voice_conversion")]
        public bool CanDoVoiceConversion { get; set; }

        [JsonProperty("can_use_style")]
        public bool CanUseStyle { get; set; }

        [JsonProperty("can_use_speaker_boost")]
        public bool CanUseSpeakerBoost { get; set; }

        [JsonProperty("serves_pro_voices")]
        public bool ServesProVoices { get; set; }

        [JsonProperty("token_cost_factor")]
        public double TokenCostFactor { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("requires_alpha_access")]
        public bool RequiresAlphaAccess { get; set; }

        [JsonProperty("max_characters_request_free_user")]
        public int MaxCharactersRequestFreeUser { get; set; }

        [JsonProperty("max_characters_request_subscribed_user")]
        public int MaxCharactersRequestSubscribedUser { get; set; }

        [JsonProperty("languages")]
        public List<Language> Languages { get; set; }

        public string AllAbout
        {
            get
            {
                return
                    $"{Name}\n" +
                    $"{Description}\n" +
                    $"Tasks: " +
                    $"Voice Conversion: {CanDoVoiceConversion == false : \"(coming soon)\"}" +
                    $"Languages: " +
                    $"{string.Join(", ", Languages.Select(lang => lang.Name).ToList())}";
            }
        }

        public override string ToString()
        {
            return AllAbout;
        }
    }

}
