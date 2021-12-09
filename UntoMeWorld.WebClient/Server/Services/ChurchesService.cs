using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Stores;

namespace UntoMeWorld.WebClient.Server.Services
{
    public class ChurchesService
    {
        private readonly IChurchesStore _churches;
        public ChurchesService(IChurchesStore churches)
        {
            _churches = churches;
        }

        public async Task<IEnumerable<Church>> GetAllChurches()
        {
            var churches = await _churches.All();
            return churches;
        }
        
        public async Task<IEnumerable<Church>> GetChurchesByQuery(string query)
        {
            var church = await _churches.All(query);
            return church;
        }

        public async Task<Church> AddChurch(Church church)
        {
            await _churches.Add(church);;
            return church;
        }
        public async Task<IEnumerable<Church>> AddChurch(List<Church> church)
        {
            await _churches.Add(church);;
            return church;
        }
        public async Task<Church> UpdateChurch(Church church)
        {
            await _churches.Add(church);
            return church;
        }
        public async Task<IEnumerable<Church>> UpdateChurch(List<Church> church)
        {
            await _churches.Update(church);
            return church;
        }
        public Task DeleteChurch(string id)
        {
            return _churches.Delete(new Church {Id = id});
        }
        public Task DeleteChurch(IEnumerable<string> ids)
        {
            return _churches.Delete(ids.Select(id => new Church {Id = id}));
        }
    }
}