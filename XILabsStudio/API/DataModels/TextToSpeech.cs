using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XILabsStudio.API.DataModels
{
    public class TextToSpeech
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("model_id")]
        public string ModelID { get; set; }

        [JsonPropertyName("voice_settings")]
        public VoiceSettings VoiceSettings { get; set; }
    }
}
