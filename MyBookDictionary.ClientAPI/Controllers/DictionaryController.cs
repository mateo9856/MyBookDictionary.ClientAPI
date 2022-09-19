using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBookDictionary.Application.Requests.Dictionary;

namespace MyBookDictionary.ClientAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DictionaryController : ControllerBase
    {
        public DictionaryController()
        {

        }

        [HttpGet("findNote/{keyphrase}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FindNote([FromQuery]string keyphrase)
        {
            //NOTE: Implement find algorithm and get by keyword
            return Ok();
        }

        [HttpGet("findNote")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FindByTags([FromBody]FindByTags tags)
        {
            //NOTE: Implement find algorithm and get by tags
            return Ok();
        }
    }
}
