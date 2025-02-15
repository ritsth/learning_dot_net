using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories;
using Play.Contracts;


namespace Play.Catalog.Service.Controllers
{
    //Atributes of the class []
    [ApiController]
    [Route("items")] //handles routes starting with /items
    public class ItemsController : ControllerBase
    {
        // public static readonly List<ItemDto> items = new()
        // {
        //     new ItemDto(Guid.NewGuid(), "portion", "Restore", 5, DateTimeOffset.UtcNow),
        //     new ItemDto(Guid.NewGuid(), "portion2", "Restore2", 10, DateTimeOffset.UtcNow),
        //     new ItemDto(Guid.NewGuid(), "portion3", "Restore3", 20, DateTimeOffset.UtcNow),
        // };


        public static int requestCounter=0;

        //to publish a message that item has been created to mesasage broker (RabbitMQ) 
        //so that other microservices can consume it
        private readonly IPublishEndpoint publishEndpoint;

        //Dependency injection by creating an interface file 
        //make an constructor
        public readonly IItemsRepository itemsRepository;
        public ItemsController(IItemsRepository itemsRepository,IPublishEndpoint publishEndpoint){
            this.itemsRepository = itemsRepository;
            this.publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAsync()
        {
            //simulating temporal failure
            // requestCounter++;
            // Console.WriteLine($"Request {requestCounter}: Starting");

            // if(requestCounter<= 2){
            //     Console.WriteLine($"Request {requestCounter}: Delaying");
            //     await Task.Delay(TimeSpan.FromSeconds(10));
            // }

            // if(requestCounter <=4){
            //     Console.WriteLine($"Request {requestCounter}: 500 (Internal Server error)");
            //     return StatusCode(500);

            // }

            var items = (await itemsRepository.GetAllAsync()).Select(item => item.AsDtos());
            return Ok(items);

        }

        [HttpGet("{id}")] //GET items/{id}
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            // var item = items.Where(item => item.Id == id).SingleOrDefault();
            var item = await  itemsRepository.GetAsync(id);
            if(item == null){
                return NotFound();
            }

            return item.AsDtos();
        }

        [HttpPost]
        //public DataType<Template> Name(params)
        //ItemDto template/record defined in Dtos.cs
        public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDTo createItemDTo)
        {
            var item = new Item
            {
                Name = createItemDTo.Name, 
                Description = createItemDTo.Description, 
                Price = createItemDTo.Price, 
                CreatedDate = DateTimeOffset.UtcNow
            };

            await itemsRepository.CreateAsync(item);

            await publishEndpoint.Publish(new CatalogItemCreated(item.Id, item.Name, item.Description));

            // Log the route values and item
            Console.WriteLine($"Route Values: id = {item.Id}");
            Console.WriteLine($"Item: {System.Text.Json.JsonSerializer.Serialize(item)}");


            // return CreatedAtAction(nameof(GetByIdAsync),new {id = item.Id}, item); //error
            return Ok(item);
        }

        [HttpPut("{id}")]
        //public DataType Name
        //No content return type: IActionResult
        public async Task<IActionResult> PutAsync(Guid id, UpdateItemDto updatedItemDto)
        {
            var existingItem = await itemsRepository.GetAsync(id);

            if(existingItem == null){
                return NotFound();
            }

            existingItem.Name= updatedItemDto.Name;
            existingItem.Description = updatedItemDto.Description;
            existingItem.Price = updatedItemDto.Price;

            await itemsRepository.UpdateAsync(existingItem);

            await publishEndpoint.Publish(new CatalogItemUpdated(existingItem.Id, existingItem.Name, existingItem.Description));

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id){

            var item = await  itemsRepository.GetAsync(id);
            if(item == null){
                return NotFound();
            }

            await itemsRepository.RemoveAsync(item.Id);

            await publishEndpoint.Publish(new CatalogItemDeleted(item.Id));

            return NoContent();
        }


    }
}