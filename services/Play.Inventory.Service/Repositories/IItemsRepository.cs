using Play.Inventory.Service.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Play.Inventory.Service.Repositories
{
    public interface IItemsRepository
    {
        Task<IReadOnlyCollection<InventoryItem>> GetAllAsync(Func<InventoryItem, bool> predicate);
        Task<InventoryItem> GetAsync(Func<InventoryItem, bool> predicate);
        Task CreateAsync(InventoryItem entity);
        Task UpdateAsync(InventoryItem entity);
        Task RemoveAsync(Guid id);
    }
}
