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
        private readonly HttpClient _client;

        public ChurchesRepository(HttpClient client)
        {
            _client = client;
        }
        
        public async Task<Church> Add(Church item)
        {
            return await _client.PostJsonAsync<Church>("/churches", item);
        }

        public async Task<Church> Update(Church item)
        {
            return await _client.PutJsonAsync<Church>("/churches", item);
        }

        public async Task Delete(Church item)
        {
            await _client.DeleteJsonAsync<Church>("/churches", item);
        }

        public Task<IEnumerable<Church>> All()
        {
            return _client.GetJsonAsync<IEnumerable<Church>>("/churches/All");
        }

        public Task<IEnumerable<Church>> All(string query)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Church>> All(Predicate<Church> query)
        {
            throw new NotImplementedException();
        }
    }
}