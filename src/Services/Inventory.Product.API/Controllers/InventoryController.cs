using Infrastructure.Common.Models;
using Inventory.Product.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Inventory;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Inventory.Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        /// <summary>
        /// api/inventory/items/{itemNo}
        /// </summary>
        /// <param name="itemNo"></param>
        /// <returns></returns>
        [Route("items/{itemNo}", Name = "GetAllByItemNo")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InventoryEntryDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<InventoryEntryDto>>> GetAllItemNo([Required] string itemNo)
        {
            var result = await _inventoryService.GetAllByItemNoAsync(itemNo);
            return Ok(result);
        }

        /// <summary>
        /// api/inventory/items/{itemNo}/paging
        /// </summary>
        /// <param name="itemNo"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("items/{itemNo}/paging", Name = "GetAllByItemNoPaging")]
        [HttpGet]
        [ProducesResponseType(typeof(PagedList<InventoryEntryDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PagedList<InventoryEntryDto>>> GetAllByItemNoPaging([Required] string itemNo, [FromQuery] GetInventoryPagingQuery query)
        {
            query.SetItemNo(itemNo);
            var result = await _inventoryService.GetAllByItemNoPagingAsync(query);
            return Ok(result);
        }

        /// <summary>
        /// api/inventory/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}", Name = "GetInventoryById")]
        [HttpGet]
        [ProducesResponseType(typeof(InventoryEntryDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<InventoryEntryDto>> GetInventoryById([Required] string id)
        {
            var result = await _inventoryService.GetByIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// api/inventory/purchase/{itemNo}
        /// </summary>
        /// <param name="itemNo"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("purchase/{itemNo}", Name = "PurchaseOrder")]
        [ProducesResponseType(typeof(InventoryEntryDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<InventoryEntryDto>> PurchaseOrder([Required] string itemNo, [FromBody] PurchaseProductDto model)
        {
            var result = await _inventoryService.PurchaseItemAsync(itemNo, model);
            return Ok(result);
        }

        /// <summary>
        /// api/inventory/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = "DeleteById")]
        [ProducesResponseType(typeof(InventoryEntryDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<InventoryEntryDto>> DeleteById([Required] string id)
        {
            var entity = await _inventoryService.GetByIdAsync(id);
            if (entity == null) return NotFound();

            await _inventoryService.DeleteAsync(id);
            return Ok();
        }
    }
}
