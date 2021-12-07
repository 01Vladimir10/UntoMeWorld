using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UntoMeWorld.WebClient.Client.Utils;
using UntoMeWorld.WebClient.Shared.Model;

namespace UntoMeWorld.WebClient.Client.Repositories
{
    public class ChurchesRepository : IRepository<Church>
    {
        private const string ChurchesEndpoint = "api/churches";
        private readonly HttpClient _client;

        public ChurchesRepository(HttpClient client)
        {
            _client = client;
        }
        
        public async Task<Church> Add(Church item)
        {
            var response = await _client.PostJsonAsync<Church>(ChurchesEndpoint, item);
            return response.IsSuccessful ? response.Data : null;
        }

        public async Task<Church> Update(Church item)
        {
            var response = await _client.PutJsonAsync<Church>(ChurchesEndpoint, item);
            return response.IsSuccessful ? response.Data : null;
        }

        public async Task Delete(Church item)
        {
            await _client.DeleteJsonAsync<Church>(ChurchesEndpoint, item);
        }

        public async Task<IEnumerable<Church>> All()
        {
            var response = await _client.GetJsonAsync<IEnumerable<Church>>(ChurchesEndpoint);
            return response.IsSuccessful ? response.Data : null;
        }

        public async Task<IEnumerable<Church>> All(string query)
        {
            var response = await _client.GetJsonAsync<IEnumerable<Church>>(ChurchesEndpoint + $"?query={query}");
            return response.IsSuccessful ? response.Data : null;
        }

        public Task<IEnumerable<Church>> All(Predicate<Church> query)
        {
            throw new NotImplementedException();
        }
    }
}