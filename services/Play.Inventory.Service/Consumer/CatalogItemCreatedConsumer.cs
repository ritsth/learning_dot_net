using MassTransit;
using Play.Contracts;
using Play.Inventory.Service.Entities;
using Play.Inventory.Service.Repositories;

namespace Play.Inventory.Service.Consumer
{
    //we make class for each message we want to consume
    public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
    {

        public readonly ICatalogItemsRepository catalogItemsRepository;

        public CatalogItemCreatedConsumer(ICatalogItemsRepository catalogItemsRepository){
            this.catalogItemsRepository= catalogItemsRepository;
        }

        public async Task Consume(ConsumeContext<CatalogItemCreated> context)
        {
            var message = context.Message;
            Console.WriteLine($"Item Created: {message.ItemId}, {message.Name}, {message.Description}");

            //Add to Catalog database in the Inventory database
            var item = await catalogItemsRepository.GetAsync(message.ItemId);

            //already in the database
            if(item != null){
                return;  
            }
            
            item = new CatalogItem
            {
                Id = message.ItemId,
                Name = message.Name,
                Description = message.Description
            };

            await catalogItemsRepository.CreateAsync(item);

            return;
        }
    }    
}

