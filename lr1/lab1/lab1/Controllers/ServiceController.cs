using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace lab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private static int result;
        private static Stack<int> results = new Stack<int>();

        [HttpGet]
        public IActionResult GetResult()
        {
            if (results.Count != 0)
                return Ok( new {RESULT = result + results.Peek()});
            else
                return Ok(new { RESULT = result });
        }

        [HttpPost("{RESULT}")]
        public IActionResult PostResult(int RESULT)
        {
            result = RESULT;
            //RESULT += results.Peek();

            return Ok(new { RESULT = result});
            
        }

        [HttpPut("{ADD}")]
        public IActionResult PutResult(int ADD)
        {
            //result = ADD;

            results.Push(ADD);

            return Ok(results.Peek());
        }

        [HttpDelete]
        public IActionResult DeleteResult()
        {
            if (results.Count > 0)
            {
                results.Pop();
                return Ok(results.Count());
            }
            return BadRequest("Stack is empty");
        }
    }
}
