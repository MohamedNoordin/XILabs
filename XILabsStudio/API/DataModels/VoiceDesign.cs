using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XILabsStudio.API.DataModels
{
    public class VoiceDesign
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("voice_description")]
        public string VoiceDescription { get; set; }

        [JsonProperty("gender")]
        public Gender Gender { get; set; }
        [JsonProperty("age")]
        public Age Age { get; set; }
        [JsonProperty("accent")]
        public Accent Accent { get; set; }

        [JsonProperty("accent_strength")]
        public float AccentStrength { get; set; }

        [JsonProperty("generated_voice_id")]
        public string GeneratedVoiceID { get; set; }
        [JsonProperty("audio")]
        public Stream Audio { get; set; }
    }
}
