using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlumnoController : ControllerBase
    {

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            ML.Alumno alumno = new ML.Alumno();
            ML.Result result = BL.Alumno.GetAll(alumno);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("GetAll")]
        public IActionResult GetAll([FromBody] ML.Alumno alumno)
        {

            ML.Result result = BL.Alumno.GetAll(alumno);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("GetbyId/{idAlumno}")]
        public IActionResult Get(int idAlumno)
        {
            ML.Result result = BL.Alumno.GetById(idAlumno);

            //ML.Usuario usuario = (ML.Usuario)result.Object;

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("Add")]
        public IActionResult Post([FromBody] ML.Alumno alumno)
        {
            ML.Result result = BL.Alumno.Add(alumno);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("Update")]
        public IActionResult Put([FromBody] ML.Alumno alumno)
        {
            ML.Result result = BL.Alumno.Update(alumno);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        // DELETE api/<UsuarioController>/5
        [HttpDelete("Delete/{IdAlumno}")]
        public IActionResult Delete(int IdAlumno)
        {
            ML.Result result = BL.Alumno.Delete(IdAlumno);

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
