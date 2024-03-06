using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using XILabsStudio.API.DataModels;

namespace XILabsStudio.API
{
    [ObservableObject]
    internal partial class XIOpenAPI
    {
        public static string Version { get; set; } = "1.0";
        public static string OAS { get; set;} = "3.0";

        public static bool IsInitialized()
        {
            return SecureStorage.Default.GetAsync("xi-api-key").Result != null ? true : false;
        }

        public static async Task<bool> ConnectAsync(string xiAPIKey)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders
    .Accept.Add(
    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("xi-api-key", xiAPIKey);

                HttpResponseMessage response = await client.GetAsync(Endpoints.User);

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        await SecureStorage.Default.SetAsync("xi-api-key", xiAPIKey);
                    }
                    catch (Exception)
                    {
                        await SecureStorage.Default.SetAsync("xi-api-key", xiAPIKey);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private string xiAPIKey;
        private TtsAPI ttsAPI;
        private UserAPI userAPI;
        private User user;
        private VoicesAPI voicesAPI;
        private HistoryAPI historyAPI;

        public XIOpenAPI(string xiAPIKey)
        {
            this.xiAPIKey = xiAPIKey;

            ttsAPI = new TtsAPI(xiAPIKey);
            userAPI = new UserAPI(xiAPIKey);
            voicesAPI = new VoicesAPI(xiAPIKey);
            historyAPI = new HistoryAPI(xiAPIKey);
        }

        public static async Task<XIOpenAPI> InitializeAsync()
        {
            try
            {
                var xiAPIKey = await SecureStorage.Default.GetAsync("xi-api-key");
                if (xiAPIKey != null) return new XIOpenAPI(xiAPIKey);
                else return default;
                    }
            catch (Exception)
            {
                var xiAPIKey = await SecureStorage.Default.GetAsync("xi-api-key");
                if (xiAPIKey != null) return new XIOpenAPI(xiAPIKey);
                else return default;
            }
        }

        public T GetInstanceOf<T>()
        {
            Type target = typeof(T);
            Type historyAPIType = typeof(HistoryAPI);
            Type userType = typeof(User);

            if (target.IsAssignableFrom(historyAPIType)) return (T)(object)historyAPI;
            else throw new ArgumentException($"The specified type {target.FullName} does not have an instance.");
                    }

        public async Task<List<Voice>> GetVoicesAsync()
        {
            try
            {
                return await voicesAPI.GetVoicesAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task DeleteVoiceAsync(string voiceID)
        {
            await voicesAPI.DeleteVoiceAsync(voiceID);
            }

        public async Task<List<Model>> GetModelsAsync()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("xi-api-key", xiAPIKey);
                try
                {
                    var response = await client.GetAsync(Endpoints.Models);
                    response.EnsureSuccessStatusCode();
                        List<Model> models = JsonConvert.DeserializeObject<List<Model>>(await response.Content.ReadAsStringAsync());
                    return models;
                }
                catch (HttpRequestException ex)
                {
                    // Handle the exception here, for example by logging or displaying an error message
                }
            }

                return null;
            }

        public async Task<User> GetUserAsync()
        {
            try
            {
                user = await userAPI.GetUserAsync();
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Stream> TextToSpeech(DataModels.TextToSpeech? tts, string voiceID = "21m00Tcm4TlvDq8ikWAM")
        {
                        return await ttsAPI.TextToSpeech(tts, voiceID);
        }

        public async Task<HistoryResponse> GetHistoryAsync(int pageSize = 100, string startAfterHistoryItemID = "")
        {
            return await historyAPI.GetHistoryAsync(pageSize, startAfterHistoryItemID);
        }

        public async Task<Stream> GetHistoryAudioAsync(string itemID)
    => await historyAPI.GetHistoryAudioAsync(itemID);

        public async Task<VoiceDesign> GenerateVoiceAsync(VoiceDesign voiceDesign) =>
            await voicesAPI.GenerateVoiceAsync(voiceDesign);

        public async Task<Voice> CreateVoiceAsync(VoiceDesign voiceDesign) =>
            await voicesAPI.CreateVoiceAsync(voiceDesign);

        public async Task<Voice> EditVoiceAsync(Voice voice) =>
            await voicesAPI.EditVoiceAsync(voice);

    }
}
