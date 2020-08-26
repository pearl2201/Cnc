using System.Collections.Generic;
using System.Threading.Tasks;
using Cnc.Api.Data;
using Cnc.Api.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IO;

namespace Cnc.Api.Controllers
{
    [Route("api/media")]
    [ApiController]
    public class MediaControler : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly AppSettings _appSettings;
        public MediaControler(ApplicationDbContext dbContext, IOptions<AppSettings> appSettingsOptions)
        {
            _dbContext = dbContext;
            _appSettings = appSettingsOptions.Value;
        }

        [HttpPost("uploads")]
        public ActionResult PostFormFile(IFormFile formfile)
        {
            var stream = formfile.OpenReadStream();
            FileStream fs = new FileStream($"{_appSettings.MediaFolderPath}/{formfile.FileName}", FileMode.CreateNew);
            stream.CopyTo(fs);
            fs.Close();
            return new OkObjectResult(new { url = $"/media/{_appSettings.MediaFolderPath}/{formfile.FileName}" });
        }
    }
}