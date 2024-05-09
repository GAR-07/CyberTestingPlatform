using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CyberTestingPlatform.Application.Services;

namespace CyberTestingPlatform.Resourse.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StorageController : Controller
    {
        private readonly IStorageService _storageService;

        public StorageController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [DisableRequestSizeLimit]
        [HttpPost("UploadFiles")]
        [Authorize(Roles = "Admin,Creator")]
        public async Task<IActionResult> UploadFiles()
        {
            var formCollection = await Request.ReadFormAsync();
            _storageService.ValidationImageFile(formCollection.Files[0]);

            var filePath = await _storageService.SaveFile(formCollection.Files[0]);
            
            return Ok(new { filePath });
        }
    }
}