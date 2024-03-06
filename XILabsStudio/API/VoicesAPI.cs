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
    class VoicesAPI
    {
        private HttpClient httpClient;

        public VoicesAPI(string xiAPIKey)
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders
                .Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("xi-api-key", xiAPIKey);
        }

        public async Task<List<Voice>> GetVoicesAsync()
        {
                try
                {
                    using var response = await httpClient.GetAsync(Endpoints.Voices);
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    VoicesResponse voicesResponse = JsonConvert.DeserializeObject<VoicesResponse>(content);
                    voicesResponse.Voices.Sort((x, y) => x.Category == "premade" && y.Category != "premade" ? 1 : 0);
                    return voicesResponse.Voices;
            }
                catch (HttpRequestException ex)
                {
                    // Handle the exception here, for example by logging or displaying an error message
                    return new List<Voice>();
                }
        }

        public async Task DeleteVoiceAsync(string voiceID)
        {
            await httpClient.DeleteAsync($"{Endpoints.Voices}/{voiceID}");
        }

        public async Task<VoiceDesign> GenerateVoiceAsync(VoiceDesign voiceDesign)
        {
            HttpResponseMessage response = await httpClient.PostAsync(Endpoints.GenerateVoice, new StringContent(JsonConvert.SerializeObject(voiceDesign), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            return new VoiceDesign
            {
                Name = voiceDesign.Name,
                VoiceDescription = voiceDesign.VoiceDescription,
                Text = voiceDesign.Text,
                Gender = voiceDesign.Gender,
                Age = voiceDesign.Age,
                Accent = voiceDesign.Accent,
                AccentStrength = voiceDesign.AccentStrength,
                GeneratedVoiceID = response.Headers.GetValues("generated_voice_id").FirstOrDefault(),
                Audio = await response.Content.ReadAsStreamAsync()
            };
        }

        public async Task<Voice> CreateVoiceAsync(VoiceDesign voiceDesign)
        {
            if (voiceDesign != null && voiceDesign.GeneratedVoiceID is null)
                voiceDesign = await GenerateVoiceAsync(voiceDesign);

            HttpResponseMessage response = await httpClient.PostAsync(Endpoints.CreateVoice,
                new StringContent(JsonConvert.SerializeObject(
                    new
                    {
                        voice_name = voiceDesign.Name,
                        generated_voice_id = voiceDesign.GeneratedVoiceID,
                        voice_description = voiceDesign.VoiceDescription
                    }), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            return new Voice
            {
                VoiceID = voiceDesign.GeneratedVoiceID
            };
        }

        public async Task<Voice> EditVoiceAsync(Voice voice)
        {
            using (MultipartFormDataContent content = new MultipartFormDataContent())
            {
                content.Add(new StringContent(voice.Name), "name");
                content.Add(new StringContent(voice.Description), "description");

                HttpResponseMessage response = await httpClient.PostAsync(Endpoints.VoiceEdit(voice.VoiceID), content);
                response.EnsureSuccessStatusCode();

                return voice;
            }
        }

        }
    }
