using System;
using System.Collections.Generic;
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

        public Task<IEnumerable<IChurch>> GetAllChurches()
        {
            return _churches.GetAll();
        }
        
        public Task<IEnumerable<IChurch>> GetChurchesByQuery(string query)
        {
            return _churches.GetByQuery(query);
        }

        public async Task<IChurch> AddChurch(IChurch church)
        {
            return await _churches.Insert(church);;
        }
        public async Task<IChurch> UpdateChurch(IChurch church)
        {
            await _churches.Insert(church);
            return church;
        }
        public Task DeleteChurch(IChurch church)
        {
            return _churches.Remove(church);
        }
    }
}