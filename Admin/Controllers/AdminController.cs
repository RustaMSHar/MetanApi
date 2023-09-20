using MetanApi.Services;
using MetanApi.Models;
using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using MetanApi.Admin.Services;

namespace MetanApi.Admin.AdminController
{
    [ApiController]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;
        public AdminController(AdminService AdminService) =>
           _adminService = AdminService;

    }

 }
