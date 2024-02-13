using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CyberTestingPlatform.Resourse.API.Models;
using CyberTestingPlatform.DataAccess;

namespace CyberTestingPlatform.Resourse.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StorageController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public StorageController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Authorize]
        [Route("GetLectures")]
        public IActionResult GetAllAccounts([FromQuery] ItemsViewModel model)
        {
            if (int.TryParse(model.SampleSize, out int sampleSize) && int.TryParse(model.Page, out int page))
            {
                try
                {
                    //var startItem = _dbContext.Accounts.Select(p => p.UserId).ToList().Count - sampleSize * page;
                    //var accountsList = _dbContext.Accounts.Skip(startItem < 0 ? 0 : startItem)
                    //    .Take(startItem < 0 ? sampleSize + startItem : sampleSize);
                    //foreach (var account in accountsList)
                    //{
                    //    account.PasswordHash = "";
                    //}
                    //return Ok(accountsList);
                    return Ok();
                }
                catch
                {
                    return BadRequest("Error getting items from the database");
                }
            }
            return BadRequest("Invalid model object");
        }
    }
}