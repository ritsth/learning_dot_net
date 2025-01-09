using MassTransit;
using Play.Contracts;

namespace Play.Inventory.Service.Consumer
{
    //we make class for each message we want to consume
    public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
    {

        // public readonly IItemsRepository itemsRepository;

        // public CatalogItemCreatedConsumer(IItemsRepository itemsRepository){
        //     this.itemsRepository = itemsRepository;
        // }

        public Task Consume(ConsumeContext<CatalogItemCreated> context)
        {
            var message = context.Message;
            Console.WriteLine($"Item Created: {message.ItemId}, {message.Name}, {message.Description}");
            return Task.CompletedTask;
        }
    }    
}

