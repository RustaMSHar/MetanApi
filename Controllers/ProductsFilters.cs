using Microsoft.AspNetCore.Mvc;
using MetanApi.Services;
using MetanApi.Models;
using MongoDB.Driver;


namespace MetanApi.Controllers
{
    [ApiController]
    [Route("api/items/filter")]

    public class ProductsFilters : ControllerBase
    {
        private readonly ItemsService _itemsService;
        public ProductsFilters(ItemsService clothesService) =>
            _itemsService = clothesService;

        [HttpGet]
        public async Task<ActionResult<List<Items>>> Get(
           [FromQuery(Name = "type")] string? type,
           [FromQuery(Name = "size")] string? size)
        {
            var filterBuilder = Builders<Items>.Filter;
            var filter = filterBuilder.Empty;

            if (!string.IsNullOrEmpty(type))
            {
                filter &= filterBuilder.Eq(x => x.Type, type);
            }

            if (!string.IsNullOrEmpty(size))
            {
                filter &= filterBuilder.AnyEq(x => x.OfSize, size);
            }
            var items = await _itemsService.GetAsync(filter);
            return items;
        }
    }
}
