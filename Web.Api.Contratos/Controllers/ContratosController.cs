using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Api.Contratos.Context;
using Web.Api.Contratos.Models;
using Web.Api.Contratos.Services;

namespace Web.Api.Contratos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContratosController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly CsvServices _csvservice;

        public ContratosController(MyDbContext context, CsvServices csvservice)
        {
            _context = context;
            _csvservice = csvservice;
        }

        // GET: api/<ContratosController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModelContratosCSV>>> Get()
        {
            return await _context.contratos.ToListAsync();
        }

        // GET api/<ContratosController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ModelContratosCSV>> Get([FromRoute] int number)
        {
            var registerExist = _context.contratos.FirstOrDefault(x => x.Contrato == number);

            if (registerExist == null)
            {
                return NotFound();
            }

            return registerExist;
        }

        // POST api/<ContratosController>
        [HttpPost]
        public async Task<ActionResult<ModelContratosCSV>> Post()
        {

            return Ok();
        }

        //post do arquivo CSV
        [HttpPost]
        public IActionResult CsvImport([FromBody] string CsvPath)
        {
            _csvservice.ImportCsv(CsvPath);
            return Ok("Arquivo importado com sucesso.");
        }

        // PUT api/<ContratosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ContratosController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ModelContratosCSV>> Delete([FromRoute] int id)
        {
            var registerExist = await _context.contratos.FindAsync(id);
            if (registerExist == null)
            {
                return NotFound(id);
            }

            _context.contratos.Remove(registerExist);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
