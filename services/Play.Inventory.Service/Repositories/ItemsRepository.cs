using MongoDB.Driver;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Repositories
{
    public class ItemsRepository : IItemsRepository
    {
        private const string collectionName= "inventoryitems";

        private readonly IMongoCollection<InventoryItem> dbCollection;

        private readonly FilterDefinitionBuilder<InventoryItem> filterBuilder = Builders<InventoryItem>.Filter;

        public ItemsRepository(){
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("Inventory");
            Console.WriteLine($"database: {database}, mongoClient: {mongoClient}");
            dbCollection= database.GetCollection<InventoryItem>(collectionName);
        }
        public async Task<IReadOnlyCollection<InventoryItem>> GetAllAsync(Func<InventoryItem, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return (await dbCollection.Find(filterBuilder.Empty).ToListAsync()).Where(predicate).ToList();
        }

        public async Task<InventoryItem> GetAsync(Func<InventoryItem, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return (await dbCollection.Find(filterBuilder.Empty).ToListAsync()).FirstOrDefault(predicate);
        }
               

        public async Task CreateAsync(InventoryItem entity)
        {
             Console.WriteLine($"problemm!!!!!!");

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(InventoryItem entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            FilterDefinition<InventoryItem> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
            await dbCollection.ReplaceOneAsync(filter, entity);
        }

        public async Task RemoveAsync(Guid id)
        {
            FilterDefinition<InventoryItem> filter = filterBuilder.Eq(entity => entity.Id, id);
            await dbCollection.DeleteOneAsync(filter);
        }



        
    }
}