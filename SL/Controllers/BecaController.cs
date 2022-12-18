using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BecaController : ControllerBase
    {

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            ML.Result result = BL.Beca.GetAll();

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
