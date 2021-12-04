using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UntoMeWorld.Domain.Model;
using UntoMeWorld.Domain.Repositories;

namespace UntoMeWorld.MongoDatabase.Repositories
{
    public class MongoChurchesRepository : IChurchesRepository
    {
        public Task<IEnumerable<IChurch>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IChurch>> GetByQuery(string query)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IChurch>> GetByQuery(Predicate<IChurch> query)
        {
            throw new NotImplementedException();
        }

        public Task<IChurch> Insert(IChurch data)
        {
            throw new NotImplementedException();
        }

        public Task<IChurch> Modify(IChurch data)
        {
            throw new NotImplementedException();
        }

        public Task Remove(IChurch data)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IChurch>> Insert(IEnumerable<IChurch> data)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IChurch>> Modify(IEnumerable<IChurch> data)
        {
            throw new NotImplementedException();
        }

        public Task Remove(IEnumerable<IChurch> data)
        {
            throw new NotImplementedException();
        }
    }
}