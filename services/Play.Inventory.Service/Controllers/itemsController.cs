using Microsoft.AspNetCore.Mvc;
using Play.Inventory.Service.Dtos;
using Play.Inventory.Service.Entities;
using Play.Inventory.Service.Repositories;
using Play.Inventory.Service.Extensions;
using Play.Inventory.Service.Clients;


namespace Play.Inventory.Service.Controllers
{
    //Atributes of the class []
    [ApiController]
    [Route("items")] //handles routes starting with /items
    public class ItemsController : ControllerBase
    {

        //Dependency injection by creating an interface file 
        //make an constructor
        public readonly IItemsRepository itemsRepository;
        private readonly CatalogClient catalogClient;

        private readonly CatalogClientI catalogClientI;

        public ItemsController(IItemsRepository itemsRepository, CatalogClient catalogClient,CatalogClientI catalogClientI){
            this.itemsRepository = itemsRepository;
            this.catalogClient= catalogClient;
            this.catalogClientI = catalogClientI;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
        {
            Console.WriteLine($"Product ID");
            await catalogClientI.GetProductFromCatalog("2");

            // Check if the userId is empty and return a BadRequest if it is
            if (userId == Guid.Empty)
            {
                return BadRequest("Invalid user ID");
            }

            // Fetch items associated with the given userId
            // var items = (await itemsRepository.GetAllAsync(item => item.UserId == userId)).Select(item => item.AsDtos());
            var catalogItems= await catalogClient.GetCatalogItemsAsync();
            var inventoryItemsEntities = await itemsRepository.GetAllAsync(item => item.UserId == userId);

            var items= inventoryItemsEntities.Select(async inventoryI => {
                var catalogI = catalogItems.Single(catalogItems => catalogItems.Id == inventoryI.CatalogItemId);
                return inventoryI.AsDtos(catalogI.Name, catalogI.Description);
            });

            // Return the list of items as DTOs
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantItemsDto grantItemsDto)
        {
            // Retrieve the inventory item based on UserId and CatalogItemId
            var inventoryItem = await itemsRepository.GetAsync( item => (item.UserId == grantItemsDto.UserId) && (item.CatalogItemId == grantItemsDto.CatalogItemId));

            // If the inventory item does not exist, create a new one
            if (inventoryItem == null)
            {
                inventoryItem = new InventoryItem
                {
                    CatalogItemId = grantItemsDto.CatalogItemId,
                    UserId = grantItemsDto.UserId,
                    Quantity = grantItemsDto.Quantity,
                    AcquiredDate = DateTimeOffset.UtcNow
                };

                // Create the new inventory item in MongoDB
                await itemsRepository.CreateAsync(inventoryItem);
            }
            else
            {
                // If the inventory item exists, update the quantity
                inventoryItem.Quantity += grantItemsDto.Quantity;

                // Update the existing inventory item in MongoDB
                await itemsRepository.UpdateAsync(inventoryItem);
            }

            // Return OK response
            return Ok();
        }


    }
}