using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Consumer
{
    public struct Walk
    {
        public int Id;
        public string Status;
        public string Name;
        public string Name1;
    }

    public class WalkClient
    {
        public async Task<List<Walk>> GetWalks(string baseUrl, HttpClient? httpClient = null)
        {
            using var client = httpClient == null ? new HttpClient() : httpClient;

            var response = await client.GetAsync(baseUrl + "/walks");
            response.EnsureSuccessStatusCode();

            var resp = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Walk>>(resp);
        }

        public async Task<Walk> GetWalkById(string baseUrl, int id, HttpClient? httpClient = null)
        {
            using var client = httpClient == null ? new HttpClient() : httpClient;

            var response = await client.GetAsync($"{baseUrl}/walk/{id}");
            response.EnsureSuccessStatusCode();

            var resp = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Walk>(resp);
        }
    }
}