using Play.Catalog.Service.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Play.Catalog.Service.Repositories
{
    public interface IItemsRepository
    {
        Task<IReadOnlyCollection<Item>> GetAllAsync();
        Task<Item> GetAsync(Guid id);
        Task CreateAsync(Item entity);
        Task UpdateAsync(Item entity);
        Task RemoveAsync(Guid id);
    }
}
