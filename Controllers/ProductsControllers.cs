using Microsoft.AspNetCore.Mvc;
using MetanApi.Services;
using MetanApi.Models;
using MongoDB.Driver;

namespace MetanApi.Controllers
{
    [ApiController]
    [Route("api/items")]
    public class ProductsControllers : ControllerBase
    {
        private readonly ItemsService _itemsService;
        public ProductsControllers(ItemsService clothesService) =>
            _itemsService = clothesService;

        [HttpGet]
        public async Task<List<Items>> Get() =>
            await _itemsService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Items>> Get(string id)
        {
            var item = await _itemsService.GetAsync(id);
            if (item is null) { return NotFound(); }
            return item;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Items newItem)
        {
            await _itemsService.CreateAsync(newItem);
            return CreatedAtAction(nameof(Get), new { id = newItem.Id }, newItem);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Items updatedItem)
        {
            var item = await _itemsService.GetAsync(id);
            if (item is null) { return NotFound(); }
            updatedItem.Id = item.Id;
            await _itemsService.UpdateAsync(id, updatedItem);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var item = await _itemsService.GetAsync(id);
            if (item is null) { return NotFound(); }
            await _itemsService.RemoveAsync(id);
            return NoContent();
        }




    }


}
