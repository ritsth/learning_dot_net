using MongoDB.Driver;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Repositories
{
    public class CatalogItemsRepository : ICatalogItemsRepository
    {
        private const string collectionName= "catalogitems";

        private readonly IMongoCollection<CatalogItem> dbCollection;

        private readonly FilterDefinitionBuilder<CatalogItem> filterBuilder = Builders<CatalogItem>.Filter;

        public CatalogItemsRepository(){
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("CatalogComsumed");
            dbCollection= database.GetCollection<CatalogItem>(collectionName);
        }
        public async Task<IReadOnlyCollection<CatalogItem>> GetAllAsync(Func<CatalogItem, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return (await dbCollection.Find(filterBuilder.Empty).ToListAsync()).Where(predicate).ToList();
        }

        public async Task<CatalogItem> GetAsync(Func<CatalogItem, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return (await dbCollection.Find(filterBuilder.Empty).ToListAsync()).FirstOrDefault(predicate);
        }
               

        public async Task CreateAsync(CatalogItem entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(CatalogItem entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            FilterDefinition<CatalogItem> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
            await dbCollection.ReplaceOneAsync(filter, entity);
        }

        public async Task RemoveAsync(Guid id)
        {
            FilterDefinition<CatalogItem> filter = filterBuilder.Eq(entity => entity.Id, id);
            await dbCollection.DeleteOneAsync(filter);
        }



        
    }
}