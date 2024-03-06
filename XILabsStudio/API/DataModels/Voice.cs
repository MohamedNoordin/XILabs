using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XILabsStudio.API.DataModels
{
    public class VoicesResponse
    {
        [JsonProperty("voices")]
        public List<Voice> Voices { get; set; }
    }

    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum Gender
    {
        [EnumMember(Value = "female")]
        Female,
        [EnumMember(Value = "male")]
        Male
    }

    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum Age
    {
        [EnumMember(Value = "young")]
        Young,
        [EnumMember(Value = "middle_aged")]
        MiddleAged,
        [EnumMember(Value = "old")]
        Old
    }

    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum Accent
    {
        [EnumMember(Value = "american")]
        American,
        [EnumMember(Value = "british")]
        British,
        [EnumMember(Value = "african")]
        African,
        [EnumMember(Value = "australian")]
        Australian,
        [EnumMember(Value = "indian")]
        Indian
    }

    public class File
    {
        [JsonProperty("file_id")]
        public string FileID { get; set; }

        [JsonProperty("file_name")]
        public string FileName { get; set; }

        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        [JsonProperty("size_bytes")]
        public int SizeBytes { get; set; }

        [JsonProperty("upload_date_unix")]
        public int UploadDateUnix { get; set; }
    }

    public class FineTuning
    {
        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("is_allowed_to_fine_tune")]
        public bool IsAllowedToFineTune { get; set; }

        [JsonProperty("fine_tuning_requested")]
        public bool FineTuningRequested { get; set; }

        [JsonProperty("finetuning_state")]
        public string FinetuningState { get; set; }

        [JsonProperty("verification_attempts")]
        public List<VerificationAttempt> VerificationAttempts { get; set; }

        [JsonProperty("verification_failures")]
        public List<string> VerificationFailures { get; set; }

        [JsonProperty("verification_attempts_count")]
        public int VerificationAttemptsCount { get; set; }

        [JsonProperty("slice_ids")]
        public List<string> SliceIDs { get; set; }

        [JsonProperty("manual_verification")]
        public ManualVerification ManualVerification { get; set; }

        [JsonProperty("manual_verification_requested")]
        public bool ManualVerificationRequested { get; set; }
    }

    public class Labels
    {
        [JsonProperty("additionalProp1")]
        public string AdditionalProp1 { get; set; }

        [JsonProperty("additionalProp2")]
        public string AdditionalProp2 { get; set; }

        [JsonProperty("additionalProp3")]
        public string AdditionalProp3 { get; set; }
    }

    public class ManualVerification
    {
        [JsonProperty("extra_text")]
        public string ExtraText { get; set; }

        [JsonProperty("request_time_unix")]
        public int RequestTimeUnix { get; set; }

        [JsonProperty("files")]
        public List<File> Files { get; set; }
    }

    public class Recording
    {
        [JsonProperty("recording_id")]
        public string RecordingID { get; set; }

        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        [JsonProperty("size_bytes")]
        public int SizeBytes { get; set; }

        [JsonProperty("upload_date_unix")]
        public int UploadDateUnix { get; set; }

        [JsonProperty("transcription")]
        public string Transcription { get; set; }
    }

    public class Sample
    {
        [JsonProperty("sample_id")]
        public string SampleID { get; set; }

        [JsonProperty("file_name")]
        public string FileName { get; set; }

        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        [JsonProperty("size_bytes")]
        public int SizeBytes { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }
    }

    public class Settings
    {
        [JsonProperty("stability")]
        public double Stability { get; set; }

        [JsonProperty("similarity_boost")]
        public double SimilarityBoost { get; set; }

        [JsonProperty("style")]
        public double Style { get; set; }

        [JsonProperty("use_speaker_boost")]
        public bool UseSpeakerBoost { get; set; }
    }

    public class Sharing
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("history_item_sample_id")]
        public string HistoryItemSampleID { get; set; }

        [JsonProperty("original_voice_id")]
        public string OriginalVoiceID { get; set; }

        [JsonProperty("public_owner_id")]
        public string PublicOwnerID { get; set; }

        [JsonProperty("liked_by_count")]
        public int LikedByCount { get; set; }

        [JsonProperty("cloned_by_count")]
        public int ClonedByCount { get; set; }

        [JsonProperty("whitelisted_emails")]
        public List<string> WhitelistedEmails { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("labels")]
        public Labels Labels { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("review_status")]
        public string ReviewStatus { get; set; }

        [JsonProperty("review_message")]
        public string ReviewMessage { get; set; }

        [JsonProperty("enabled_in_library")]
        public bool EnabledInLibrary { get; set; }
    }

    public class VerificationAttempt
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("date_unix")]
        public int DateUnix { get; set; }

        [JsonProperty("accepted")]
        public bool Accepted { get; set; }

        [JsonProperty("similarity")]
        public int Similarity { get; set; }

        [JsonProperty("levenshtein_distance")]
        public int LevenshteinDistance { get; set; }

        [JsonProperty("recording")]
        public Recording Recording { get; set; }
    }

    public class Voice
    {
        [JsonProperty("voice_id")]
        public string VoiceID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public string Title
        {
            get { return $"{Category}/{Name}"; }
        }

        [JsonProperty("samples")]
        public List<Sample> Samples { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("fine_tuning")]
        public FineTuning FineTuning { get; set; }

        [JsonProperty("labels")]
        public Labels Labels { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("preview_url")]
        public string PreviewUrl { get; set; }

        [JsonProperty("available_for_tiers")]
        public List<string> AvailableForTiers { get; set; }

        [JsonProperty("settings")]
        public Settings Settings { get; set; }

        [JsonProperty("sharing")]
        public Sharing Sharing { get; set; }

        [JsonProperty("high_quality_base_model_ids")]
        public List<string> HighQualityBaseModelIDs { get; set; }

        public override string ToString()
        {
            return
                $"{Name} " +
                $"{Labels.AdditionalProp1} {Labels.AdditionalProp2} {Labels.AdditionalProp3}" +
                $"{Description}";
        }
    }


}
