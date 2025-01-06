using Play.Inventory.Service.Dtos;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Extensions
{
    public static class Extensions{
        public static InventoryItemDto AsDtos(this InventoryItem item, string Name,string Description)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            return new InventoryItemDto(item.CatalogItemId,item.Quantity,Name, Description,item.AcquiredDate);
        }
    }
}