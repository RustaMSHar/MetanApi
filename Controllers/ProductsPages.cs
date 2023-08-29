using Microsoft.AspNetCore.Mvc;
using MetanApi.Services;
using MetanApi.Models;
using MongoDB.Driver;

namespace MetanApi.Controllers
{
    [ApiController]
    [Route("api/items/filter/page")]
    public class ProductsPages : ControllerBase
    {
        private readonly ItemsService _itemsService;
        public ProductsPages(ItemsService clothesService) =>
            _itemsService = clothesService;
        

    }
}
