using MongoDB.Driver;
using MongoDB.Driver.Linq;
using UntoMeWorld.Application.Stores;
using UntoMeWorld.Domain.Common;
using UntoMeWorld.Domain.Model.Abstractions;
using UntoMeWorld.Infrastructure.Helpers;
using UntoMeWorld.Infrastructure.Services;

namespace UntoMeWorld.Infrastructure.Stores
{
    public abstract class GenericMongoStore<TModel, TReadModel> : IStore<TModel>
        where TModel : class, IModel, IRecyclableModel
        where TReadModel : class
    {
        protected readonly IMongoCollection<TModel> Collection;
        private readonly List<IPipelineStageDefinition> _pipelineStages;
        private readonly Func<TReadModel, TModel> _modelConverter;

        protected GenericMongoStore(MongoDbService service, string collection,
            Func<TReadModel, TModel> modelConverter = null, List<IPipelineStageDefinition> pipelineStages = null)
        {
            _modelConverter = modelConverter ?? (x => x as TModel);
            _pipelineStages = pipelineStages;
            Collection = service.GetCollection<TModel>(collection);
        }

        public async Task<IEnumerable<TModel>> All()
        {
            try
            {
                return await Collection.Find(_ => true)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<TModel>> All(string query)
        {
            var items = await Collection.FindAsync(Builders<TModel>.Filter.Text(query));
            return await items.ToListAsync();
        }

        public async Task<IEnumerable<TModel>> All(Predicate<TModel> query)
        {
            var result = await Collection.FindAsync(_ => query(_));
            return await result.ToListAsync();
        }

        public async Task<PaginationResult<TModel>> Query(QueryFilter filter, 
            string textQuery = null,
            string orderBy = null,
            bool orderDesc = false, int page = 1, int pageSize = 100)
        {
            var (totalItems, result) =
                await Collection.QueryByPageAndSort<TModel, TReadModel>(filter, textQuery, orderBy ?? string.Empty, orderDesc, page, pageSize,
                    _pipelineStages);
            return new PaginationResult<TModel>
            {
                Result = result.Select(_modelConverter).ToList(),
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                TotalItems = totalItems,
                Page = page
            };
        }

        public async Task<TModel> AddOne(TModel church)
        {
            await Collection.InsertOneAsync(church);
            return church;
        }

        public async Task<TModel> UpdateOne(TModel data)
        {
            data.LastUpdatedOn = DateTime.UtcNow;
            await Collection.ReplaceOneAsync(Builders<TModel>.Filter.Eq(c => c.Id, data.Id), data);
            return data;
        }

        public Task DeleteOne(string key)
            => Collection.UpdateOneAsync(Builders<TModel>.Filter.Eq(m => m.Id, key),
                Builders<TModel>.Update.Set(m => m.IsDeleted, true)
                    .Set(m => m.DeletedOn, DateTime.UtcNow));

        public Task PurgeOne(string key)
            => Collection.DeleteOneAsync(Builders<TModel>.Filter.Eq(m => m.Id, key));

        public Task RestoreOne(string key)
            => Collection.UpdateOneAsync(Builders<TModel>.Filter.Eq(m => m.Id, key),
                Builders<TModel>.Update.Set(m => m.IsDeleted, false)
                    .Set(m => m.LastUpdatedOn, DateTime.UtcNow));


        public async Task<IEnumerable<TModel>> AddMany(List<TModel> data)
        {
            await Collection.InsertManyAsync(data);
            return data;
        }

        public async Task<IEnumerable<TModel>> UpdateMany(List<TModel> data)
        {
            var tasks =
                from item in data
                let filter = Builders<TModel>.Filter.Eq(c => c.Id, item.Id)
                select new ReplaceOneModel<TModel>(filter, item);
            await Collection.BulkWriteAsync(tasks);
            return data;
        }

        public Task DeleteMany(IEnumerable<string> keys)
            => Collection
                .UpdateManyAsync(Builders<TModel>.Filter.In(m => m.Id, keys.Distinct()), Builders<TModel>
                    .Update.Set(m => m.IsDeleted, true)
                    .Set(m => m.DeletedOn, DateTime.UtcNow));

        public Task PurgeMany(IEnumerable<string> keys)
            => Collection.DeleteManyAsync(Builders<TModel>.Filter.In(_ => _.Id, keys));

        public Task RestoreMany(IEnumerable<string> keys)
            => Collection
                .UpdateManyAsync(Builders<TModel>.Filter.In(m => m.Id, keys.Distinct()), Builders<TModel>
                    .Update.Set(m => m.IsDeleted, false)
                    .Set(m => m.DeletedOn, DateTime.UtcNow));

        public async Task<TModel> Get(string id)
        {
            if (_pipelineStages == null)
                return await Collection.AsQueryable().FirstOrDefaultAsync(i => i.Id == id);
            var dataPipeline =
                PipelineDefinition<TModel, TReadModel>.Create(_pipelineStages);
            var facet = AggregateFacet.Create("data", dataPipeline);
            var aggregation = await Collection
                .Aggregate()
                .Match(i => i.Id == id)
                .Facet(facet)
                .ToListAsync();
            var facets = aggregation.FirstOrDefault()
                ?.Facets;
            if (facets == null || !facets.Any())
                return null;
            var result = facets[0].Output<TReadModel>();
            return result == null || !result.Any() ? null : _modelConverter(result[0]);
        }
    }

    public abstract class GenericMongoStore<TModel> : GenericMongoStore<TModel, TModel>
        where TModel : class, IModel, IRecyclableModel
    {
        protected GenericMongoStore(MongoDbService service, string collection) :
            base(service, collection, m => m)
        {
        }
    }
}