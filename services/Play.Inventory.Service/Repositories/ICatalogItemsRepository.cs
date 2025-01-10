using Play.Inventory.Service.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Play.Inventory.Service.Repositories
{
    public interface ICatalogItemsRepository
    {

        Task<IReadOnlyCollection<CatalogItem>> GetAllAsync();
        Task<CatalogItem> GetAsync(Guid id);
        Task CreateAsync(CatalogItem entity);
        Task UpdateAsync(CatalogItem entity);
        Task RemoveAsync(Guid id);
    }
}
