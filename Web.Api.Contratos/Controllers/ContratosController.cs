using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Api.Contratos.Context;
using Web.Api.Contratos.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Api.Contratos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContratosController : ControllerBase
    {
        private readonly MyDbContext _context;

        public ContratosController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/<ContratosController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModelContratos>>> Get()
        {
            return await _context.Contratos.ToListAsync();
        }

        // GET api/<ContratosController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ModelContratos>> Get([FromRoute] int number)
        {
            var registerExist = _context.Contratos.FirstOrDefault(x => x.Contrato == number);

            if (registerExist == null)
            {
                return NotFound();
            }

            return registerExist;
        }

        // POST api/<ContratosController>
        [HttpPost]
        public async Task<ActionResult<ModelContratos>> Post([FromBody] string value)
        {

            return Ok();
        }

        // PUT api/<ContratosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ContratosController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ModelContratos>> Delete([FromRoute] int id)
        {
            var registerExist = await _context.Contratos.FindAsync(id);
            if (registerExist == null)
            {
                return NotFound(id);
            }

            _context.Contratos.Remove(registerExist);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
