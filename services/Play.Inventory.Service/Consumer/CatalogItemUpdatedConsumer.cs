using MassTransit;
using Play.Contracts;

namespace Play.Inventory.Service.Consumer
{
    //we make class for each message we want to consume like create, update and delete
    public class CatalogItemUpdatedConsumer : IConsumer<CatalogItemUpdated>
    {

        // public readonly IItemsRepository itemsRepository;

        // public CatalogItemUpdatedConsumer(IItemsRepository itemsRepository){
        //     this.itemsRepository = itemsRepository;
        // }

        public Task Consume(ConsumeContext<CatalogItemUpdated> context)
        {
            var message = context.Message;
            Console.WriteLine($"Item Updated: {message.ItemId}, {message.Name}, {message.Description}");
            return Task.CompletedTask;
        }
    }    
}

