using MetanApi.Admin.Services;
using MetanApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.IO;
using System.Threading.Tasks;

namespace MetanApi.Controllers
{
    
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;

        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

        // Получение товаров по типу
        [HttpGet("getItemsByType/{type}")]
        public async Task<ActionResult<List<Items>>> GetItemsByTypeAsync(string type)
        {
            var filter = Builders<Items>.Filter.Eq(x => x.Type, type);
            var items = await _adminService.GetAsync(filter);
            return Ok(items);
        }

        // Добавление нового товара
        [HttpPost("addItem")]
        public async Task<ActionResult<Items>> AddItemAsync([FromBody] Items newItem)
        {
            await _adminService.CreateAsync(newItem);
            return Ok(newItem);
        }

        // Обновление данных о товаре
        [HttpPut("updateItem/{id}")]
        public async Task<IActionResult> UpdateItemAsync(string id, [FromBody] Items updatedItem)
        {
            await _adminService.UpdateAsync(id, updatedItem);

            return Ok("Item updated successfully.");
        }

        // Удаление товара
        [HttpDelete("deleteItem/{id}")]
        public async Task<IActionResult> DeleteItemAsync(string id)
        {
            await _adminService.RemoveAsync(id);

            return Ok("Item deleted successfully.");
        }


        // Загрузка изображения и связывание с товаром
        [HttpPost("uploadImage")]
        public async Task<ActionResult<ObjectId>> UploadImageAsync(IFormFile file, [FromForm] string itemId)
        {
            using (var stream = file.OpenReadStream())
            {
                var imageId = await _adminService.UploadImageAsync(file.FileName, stream);

                if (imageId != ObjectId.Empty)
                {
                    // Свяжем изображение с товаром, передавая imageId и itemId (Id товара)
                    var success = await _adminService.LinkImageToItemAsync(itemId, imageId.ToString());

                    if (success)
                    {
                        return Ok(imageId);
                    }
                    else
                    {
                        // Обработка ошибки связывания изображения с товаром
                        return BadRequest("Failed to link image to item.");
                    }
                }
                else
                {
                    // Обработка ошибки загрузки изображения
                    return BadRequest("Failed to upload image.");
                }
            }
        }

        // Удаление изображения
        [HttpDelete("deleteImage/{id}")]
        public async Task<IActionResult> DeleteImageAsync(string id)
        {
            var (success, errorMessage) = await _adminService.DeleteImageAsync(id);

            if (success)
            {
                return Ok("Image deleted successfully.");
            }
            else
            {
                return BadRequest($"Failed to delete image: {errorMessage}");
            }
        }



    }
}
