using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CyberTestingPlatform.Resourse.API.Models;
using CyberTestingPlatform.Application.Services;
using CyberTestingPlatform.Core.Models;

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

        [HttpPost, DisableRequestSizeLimit]
        [Authorize(Roles = "Admin,Creator")]
        [Route("UploadFiles")]
        public async Task<IActionResult> UploadFiles()
        {
            var formCollection = await Request.ReadFormAsync();
            var (filePath, error) = await _storageService.SaveFile(formCollection.Files[0]);
            
            return Ok(new
            {
                filePath,
                error
            });
        }
    }
}