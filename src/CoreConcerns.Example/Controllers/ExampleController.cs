using CoreConcerns.Example.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoreConcerns.Example.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExampleController : ControllerBase
    {
        private readonly DataService _dataService;

        public ExampleController(DataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("get/{key}")]
        public async Task<IActionResult> Get(string key)
        {
            var data = await _dataService.GetCachedDataAsync(key);
            return data != null ? Ok(data) : NotFound("Data not found in cache.");
        }

        [HttpPost("set/{key}")]
        public async Task<IActionResult> Set(string key, [FromBody] string data)
        {
            await _dataService.SetDataAsync(key, data);
            return Ok($"Data set in cache for key: {key}");
        }
    }
}
