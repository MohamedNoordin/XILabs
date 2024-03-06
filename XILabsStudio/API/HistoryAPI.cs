using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using XILabsStudio.API.DataModels;

namespace XILabsStudio.API
{
    [ObservableObject]
    partial class HistoryAPI
    {
        private HttpClient httpClient;

        public HistoryAPI(string xiAPIKey)
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders
                .Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("xi-api-key", xiAPIKey);
        }

        public async Task<HistoryResponse> GetHistoryAsync(int pageSize = 100, string startAfterHistoryItemID = "")
        {
            if (pageSize <= 0 || pageSize > 1000)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size should be greater than 0 and less than 1000");

            FormUrlEncodedContent urlEncoded = null;
            if (startAfterHistoryItemID != "")
            {
                urlEncoded = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "page_size", pageSize.ToString() },
{ "start_after_history_item_id", startAfterHistoryItemID }
            });
            }
            else
            {
                urlEncoded = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "page_size", pageSize.ToString() }
                });
            }


var url = $"{Endpoints.History}?{await urlEncoded.ReadAsStringAsync()}";
            return JsonConvert.DeserializeObject<DataModels.HistoryResponse>(await httpClient.GetStringAsync(url));
        }

        public async Task<Stream> GetHistoryAudioAsync(string itemID)
        {
            HttpResponseMessage response = await httpClient.GetAsync(Endpoints.HistoryAudio(itemID));
            return await response.Content.ReadAsStreamAsync();
        }

        public async Task<Stream> DownloadAsync(List<string> itemIDs)
        {
            HttpResponseMessage response = await httpClient.PostAsync(Endpoints.HistoryDownload,
                new StringContent(
                    JsonConvert.SerializeObject(
                        new { history_item_ids = itemIDs }, Formatting.Indented), Encoding.UTF8, "application/json"));
            return await response.Content.ReadAsStreamAsync();
        }

        public async Task<bool> RemoveAsync(List<string> itemIDs)
        {
            foreach (var ID in itemIDs)
                await httpClient.DeleteAsync(Endpoints.HistoryRemove(ID));

            return true;
        }

    }
}
