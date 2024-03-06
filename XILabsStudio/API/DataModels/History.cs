using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XILabsStudio.API.DataModels
{
    [ObservableObject]
    public partial class HistoryPage
    {
        public int Number {  get; set; }

        [ObservableProperty]
        private ObservableCollection<History> histories;
    }

    public class Feedback
    {
        [JsonProperty("thumbs_up")]
        public bool ThumbsUp { get; set; }

        [JsonProperty("feedback")]
        public string FeedbackText { get; set; }

        [JsonProperty("emotions")]
        public bool Emotions { get; set; }

        [JsonProperty("inaccurate_clone")]
        public bool InaccurateClone { get; set; }

        [JsonProperty("glitches")]
        public bool Glitches { get; set; }

        [JsonProperty("audio_quality")]
        public bool AudioQuality { get; set; }

        [JsonProperty("other")]
        public bool Other { get; set; }

        [JsonProperty("review_status")]
        public string ReviewStatus { get; set; }
    }

    [ObservableObject]
    public partial class History
    {
        [JsonProperty("history_item_id")]
        public string HistoryItemId { get; set; }

        [JsonProperty("request_id")]
        public string RequestId { get; set; }

        [JsonProperty("voice_id")]
        public string VoiceId { get; set; }

        [JsonProperty("model_id")]
        public string ModelId { get; set; }

        [JsonProperty("voice_name")]
        public string VoiceName { get; set; }

        [JsonProperty("voice_category")]
        public string VoiceCategory { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("date_unix")]
        public int DateUnix { get; set; }

        public string DateCreated
        {
            get => DateTimeOffset.FromUnixTimeSeconds(DateUnix).ToString("MM.dd.yy, hh:mm");
        }

        [JsonProperty("character_count_change_from")]
        public int CharacterCountChangeFrom { get; set; }

        [JsonProperty("character_count_change_to")]
        public int CharacterCountChangeTo { get; set; }

        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        public string AllAbout
        {
            get => $"{VoiceName} - {State} - {DateCreated} - {Text}";
        }

        [JsonProperty("settings")]
        public Settings Settings { get; set; }

        [JsonProperty("feedback")]
        public Feedback Feedback { get; set; }

        [ObservableProperty]
        private bool isSelected;

        public override string ToString()
        {
            return AllAbout;
        }
    }

    public class HistoryResponse
    {
        [JsonProperty("history")]
        public List<History> History { get; set; }

        [JsonProperty("last_history_item_id")]
        public string LastHistoryItemId { get; set; }

        [JsonProperty("has_more")]
        public bool HasMore { get; set; }
    }
}
