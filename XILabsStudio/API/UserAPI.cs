using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using XILabsStudio.API.DataModels;

namespace XILabsStudio.API
{
    [ObservableObject]
    partial class UserAPI
    {
        [ObservableProperty]
        private static User current;

        private HttpClient httpClient;

        public UserAPI(string xiAPIKey)
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders
                .Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("xi-api-key", xiAPIKey);
        }

        public async Task<User> GetUserAsync()
        {
            try
            {
                var response = await httpClient.GetAsync(Endpoints.User);

                var content = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(content);
                Current = user;

                return user;
            }
            catch (HttpRequestException ex) when (ex.StatusCode == (HttpStatusCode)443)
            {
                return new User();
            }
            catch (System.Net.WebException ex)
            {
                return new User();
            }
        }
    }
}
