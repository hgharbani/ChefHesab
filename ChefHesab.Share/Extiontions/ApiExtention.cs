using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChefHesab.Share.Extiontions
{
    public class ApiExtention
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _client;

        private readonly JsonSerializerOptions _options;

        public ApiExtention(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _client = new HttpClient();
            _client= new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });
            _options = new JsonSerializerOptions
            {
                IgnoreNullValues = false,
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };
        }

        public async Task<TResponse> PostDataToApiAsync<TRequest, TResponse>(string apiUrl, TRequest data)
        {
            try
            {
                using var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.PostAsJsonAsync(apiUrl, data);
                if (response.IsSuccessStatusCode)
                {
                  
                    var result = await response.Content.ReadFromJsonAsync<TResponse>();
                    return result;
                }
                throw new ArgumentException();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
        public async Task<TResponse> PutDataToApiAsync<TRequest, TResponse>(string apiUrl, TRequest data)
        {
            try
            {
                using var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.PutAsJsonAsync(apiUrl, data);
                if (response.IsSuccessStatusCode)
                {

                    var result = await response.Content.ReadFromJsonAsync<TResponse>();
                    return result;
                }
                throw new ArgumentException();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<T> GetServiceAsync<T>(string baseUrl)
        {
            _client.DefaultRequestHeaders.Clear();
            var response = await _client.GetAsync(baseUrl);
            if (response != null)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                var serverResult = JsonSerializer.Deserialize<T>(jsonString, _options);
                return serverResult;

            }
            return default(T);
        }

        public async Task<T> GetServiceByModelAsync<T, V>(string baseUrl, V model)
        {
            StringContent content = new StringContent(JsonSerializer.Serialize(model, _options), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            HttpResponseMessage httpResponseMessage = await _client.PutAsync(baseUrl, content);
            if (httpResponseMessage != null)
            {
                var data = await httpResponseMessage.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(data))
                {
                    return default(T);
                }
                return JsonSerializer.Deserialize<T>(data, _options);
            }

            return default(T);
        }

        public async Task GetServiceAsync<T>(string baseUrl, T Model)
        {
            var jsonObject = JsonSerializer.Serialize(Model, _options);
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Clear();
            var response = await _client.PutAsync(baseUrl, content);
            if (response != null)
            {
                await response.Content.ReadAsStringAsync();
            }

        }
    }
}
