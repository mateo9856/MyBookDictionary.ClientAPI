using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBookDictionary.Application.Requests.Dictionary;
using MyBookDictionary.Application.WebSearch;
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> FindNotesFromGoogle(string keyphrase)
        {
            try
            {

                var result = await _dictionaryService.GenerateByKeywordAsync(keyphrase);
          
                if(result is FindByKeywordResponse)
                {
                    var castResult = (FindByKeywordResponse)result;

                    return castResult.Status == "Success" ? Ok(castResult) : StatusCode(500, castResult);
                }

                return StatusCode(500, "Unexpected error!");
            
            } 
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [HttpPost("generateFromAddress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GenerateFromAddress([FromBody]GenerateFromAddress address)
        {//TODO: extend ContextClass table to tag values save as bool val IsTag
            try
            {
                var generate = await _dictionaryService.GenerateNote(address.Uri);

                return Ok(generate);
            }
            catch (Exception e) 
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
