using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using XILabsStudio.API.DataModels;

namespace XILabsStudio.API
{
    internal class TtsAPI
    {
        private HttpClient httpClient;

        public TtsAPI(string xiAPIKey)
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("accept", "audio/mpeg");
            httpClient.DefaultRequestHeaders
                .Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("xi-api-key", xiAPIKey);
        }

        public async Task<Stream> TextToSpeech(
            DataModels.TextToSpeech tts,
            string voiceID = "21m00Tcm4TlvDq8ikWAM",
            int optimizeStreamingLatency = 0,
            string outputFormat = "mp3_44100_128")
        {
            if (string.IsNullOrWhiteSpace(tts.Text))
            {
                throw new ArgumentNullException("Text is required.");
            }

            if (string.IsNullOrWhiteSpace(voiceID))
            {
                throw new ArgumentNullException("voiceID is required.");
            }

            if (string.IsNullOrWhiteSpace(tts.ModelID))
            {
                throw new ArgumentNullException("modelID is required.");
            }

            if (optimizeStreamingLatency < 0 || optimizeStreamingLatency > 22)
            {
                throw new ArgumentOutOfRangeException("optimizeStreamingLatency shouldn't be less than 0 and not greater than 22.");
            }

            if (tts.VoiceSettings == null)
            {
                tts.VoiceSettings = new VoiceSettings();
            }

            var urlEncoded = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "optimize_streaming_latency", optimizeStreamingLatency.ToString() },
                { "output_format", outputFormat }
            });

            var url = $"{Endpoints.TextToSpeech}/{voiceID}?{await urlEncoded.ReadAsStringAsync()}";
            HttpResponseMessage response = await httpClient.PostAsJsonAsync<DataModels.TextToSpeech>(url, tts);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStreamAsync();
        }

        public async Task<Task<Stream>> TextToSpeechStream(
            DataModels.TextToSpeech tts,
            string voiceID = "21m00Tcm4TlvDq8ikWAM",
            int optimizeStreamingLatency = 0,
            string outputFormat = "mp3_44100_128")
        {
            if (string.IsNullOrWhiteSpace(tts.Text))
            {
                throw new ArgumentNullException("Text is required.");
            }

            if (string.IsNullOrWhiteSpace(voiceID))
            {
                throw new ArgumentNullException("voiceID is required.");
            }

            if (string.IsNullOrWhiteSpace(tts.ModelID))
            {
                throw new ArgumentNullException("modelID is required.");
            }

            if (optimizeStreamingLatency < 0 || optimizeStreamingLatency > 22)
            {
                throw new ArgumentOutOfRangeException("optimizeStreamingLatency shouldn't be less than 0 and not greater than 22.");
            }

            if (tts.VoiceSettings == null)
            {
                tts.VoiceSettings = new VoiceSettings();
            }

            var urlEncoded = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "optimize_streaming_latency", optimizeStreamingLatency.ToString() },
                { "output_format", outputFormat }
            });

            var url = $"{Endpoints.TextToSpeech}/{voiceID}/stream?{await urlEncoded.ReadAsStringAsync()}";
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(tts), Encoding.UTF8, "application/json");
                using HttpResponseMessage response = await httpClient.SendAsync(request,
                    HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                return await Task.FromResult(response.Content.ReadAsStreamAsync());
            }
        }


    }
}
