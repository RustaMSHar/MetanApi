using Microsoft.AspNetCore.Mvc;
using MetanApi.Services;
using MetanApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace MetanApi.Controllers
{
    [ApiController]
    [Route("api/items/filter")]

    public class ProductsFilters : ControllerBase
    {
        private readonly ItemsService _itemsService;

        public ProductsFilters(ItemsService itemsService)
        {
            _itemsService = itemsService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Items>>> Get(
            [FromQuery(Name = "type")] string? type,
            [FromQuery(Name = "size")] string? size,
            [FromQuery(Name = "name")] string? name,
            [FromQuery(Name = "price")] double? price,
            [FromQuery(Name = "maxprice")] double? maxprice,
            [FromQuery(Name = "minprice")] double? minprice )

        {
            var filterBuilder = Builders<Items>.Filter;
            var filter = filterBuilder.Empty;

            if (!string.IsNullOrEmpty(type)) filter &= filterBuilder.Eq(x => x.Type, type);
                
            if (!string.IsNullOrEmpty(size)) filter &= filterBuilder.AnyEq(x => x.OfSize, size);
           
            if (!string.IsNullOrEmpty(name)) filter &= filterBuilder.Regex(x => x.ItemName, new BsonRegularExpression(name, "i"));
            
            if (minprice.HasValue) filter &= filterBuilder.Gte(x => x.Price, minprice.Value);
           
            if (maxprice.HasValue) filter &= filterBuilder.Lte(x => x.Price, maxprice.Value);

            if (price.HasValue) filter &= filterBuilder.Eq(x => x.Price, price.Value);
           
            var items = await _itemsService.GetAsync(filter);
            return items;
        }
    }
}
