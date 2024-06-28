using Inventory.Product.API.Entities;
using Inventory.Product.API.Repositories.Abstraction;
using Shared.DTOs.Inventory;

namespace Inventory.Product.API.Services.Interfaces
{
    public interface IInventoryService : IMongoDbRepositoryBase<InventoryEntry>
    {
        Task<IEnumerable<InventoryEntryDto>> GetAllByItemNoAsync(string itemNo);
        Task<IEnumerable<InventoryEntryDto>> GetAllByItemNoPagingAsync();
    }
}
