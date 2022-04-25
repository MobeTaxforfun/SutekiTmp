using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SutekiTmp.ApiControllers.v2
{
    [ApiExplorerSettings(GroupName = "v2")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class Values2Controller : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> ListedData()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string GetDataById(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void PostData([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void PutData(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void DeleteData(int id)
        {
        }
    }
}
