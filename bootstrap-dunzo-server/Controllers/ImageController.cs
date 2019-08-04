using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Repository.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bootstrap_dunzo_server.Controllers
{
    [Route("api/image")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }
        [HttpPost("parse")]
        public async Task<ActionResult<ParsedData>> UploadImage([FromBody]JsonDto image)
        {

            ParsedData parsedData = await _imageService.UploadImage(image);

            return Ok(parsedData);
        }

        [HttpPost("save")]
        public ActionResult<int> SaveData([FromBody]ParsedData data)
        {     

            return Ok(_imageService.SaveData(data));
        }

        [HttpGet("metaData")]
        public ActionResult<MetaData> GetAllMeta()
        {
            
            MetaData result = _imageService.GetAllMetaData();

            return Ok(result);
        }

        [HttpGet("getAllData")]
        public ActionResult<IList<GridDataViewModel>> GetAllData()
        {

            var result = _imageService.GetAllData();

            return Ok(result);
        }       

    }
}