using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBookDictionary.Application.Requests.Dictionary;
using MyBookDictionary.Infra.Interfaces;

namespace MyBookDictionary.ClientAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DictionaryController : ControllerBase
    {
        private readonly IDictionaryService _dictionaryService;
        private readonly object _lock = new object();
        public DictionaryController(IDictionaryService dictionaryService)
        {
            _dictionaryService = dictionaryService;
        }

        [HttpGet("findNote/{keyphrase}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FindNote(string keyphrase)
        {
            try
            {
                int status = 200;

                var result = await _dictionaryService.GenerateByKeywordAsync(keyphrase);
                Console.WriteLine();
          
                return Ok();
            
            } 
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            
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
