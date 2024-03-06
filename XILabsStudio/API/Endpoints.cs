using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XILabsStudio.API
{
    internal class Endpoints
    {
        public static string APIName { get; } = "https://api.elevenlabs.io";
        public static string VerName { get; } = "v1";

        public static string TextToSpeech { get; }
    = $"{APIName}/{VerName}/text-to-speech";

        public static string Voices { get; }
    = $"{APIName}/{VerName}/voices";

        public static string User { get; }
    = $"{APIName}/{VerName}/user";

                public static string Models { get; }
            = $"{APIName}/{VerName}/models";

                public static string History { get; }
            = $"{APIName}/{VerName}/history";

                public static string HistoryDownload { get; }
            = $"{History}/download";

        public static string HistoryRemove(string itemID)
    => $"{History}/{itemID}";

        public static string HistoryAudio(string itemID)
    => $"{History}/{itemID}/audio";

                public static string CreateVoice { get; }
            = $"{APIName}/{VerName}/voice-generation/create-voice";

                public static string GenerateVoice { get; }
            = $"{APIName}/{VerName}/voice-generation/generate-voice";

                public static string CloneVoice { get; }
            = $"{Voices}/add";

                public static string VoiceEdit(string voiceID) =>
            $"{Voices}/{voiceID}/edit";

        /*        public static string  { get; }
            = $"{APIName}/{VerName}/";*/

        public class Website
        {
            public static string ElevenLabs { get; }
            = "https://elevenlabs.io/";

            public static string Subscription { get; }
            = "https://elevenlabs.io/subscription";

            public static string VoiceLibrary { get; }
            = "https://elevenlabs.io/app/voice-library";

            public static string VoiceLab { get; }
            = "https://elevenlabs.io/voice-cloning";

            public static string Dubbing { get; }
            = "https://elevenlabs.io/app/dubbing";

            public static string Projects { get; }
            = "https://elevenlabs.io/app/projects";

            public static string Usage { get; }
            = "https://elevenlabs.io/usage";

        }
    }
}
