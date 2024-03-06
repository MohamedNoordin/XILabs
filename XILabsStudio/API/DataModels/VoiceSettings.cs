using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XILabsStudio.API.DataModels
{
    public class VoiceSettings
    {
        [JsonPropertyName("stability")]
        public double Stability { get; set; } = 0.5;

        [JsonPropertyName("similarity_boost")]
        public double SimilarityBoost { get; set; } = 0.75;

        [JsonPropertyName("style")]
        public double Style { get; set; } = 0;

        [JsonPropertyName("use_speaker_boost")]
        public bool UseSpeakerBoost { get; set; } = true;
    }
}
