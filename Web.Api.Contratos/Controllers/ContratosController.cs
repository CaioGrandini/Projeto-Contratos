using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using Web.Api.Contratos.Context;
using Web.Api.Contratos.Models;
using Web.Api.Contratos.Services;
using Web.Api.Contratos.ViewModel;

namespace Web.Api.Contratos.Controllers
{
    [Authorize]
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

        // GET: api/<ContratosController>
        [HttpGet("arquivos")]
        public async Task<ActionResult<IEnumerable<ModelArquivos>>> GetArquivos()
        {
            return await _context.arquivos.ToListAsync();
        }

        // GET api/<ContratosController>/5
        [HttpGet("{number}")]
        public async Task<ActionResult<ContratoUsuarioViewModel>> Get([FromRoute] string number)
         {
            var registerExist = _context.contratos.Where(x => x.CPF == number);

            if (registerExist == null)
            {
                return NotFound();
            }

            int atraso = 0;

            foreach (var item in registerExist)
            {
                var dataregistro = Convert.ToDateTime(item.Vencimento);
                var diferenca = DateTime.Now - dataregistro;
                var dias = diferenca.Days;

                if(dias > atraso)
                {
                    atraso = dias;
                }
            }
            var contratoUsuario = new ContratoUsuarioViewModel
            {
                //Sum -> comando para somar todos os valores
                Valor = registerExist.Sum(x => x.Valor),
                Atraso = atraso
            };

            return contratoUsuario;
        }

        [HttpPost("csv")]
        public async Task<IActionResult> UploadFile()
        {
            var file = Request.Form.Files[0];
            if (file == null || file.Length <= 0)
                return BadRequest("Nenhum arquivo foi enviado.");

            try
            {
                //para pegar os valores em virgula 
                var configuration = new CsvConfiguration(new CultureInfo("pt-BR"))
                {
                    //para pegar acentuacao correta
                    Encoding = Encoding.GetEncoding("iso-8859-1"), 
                    Delimiter = ";",

                };
                List<ModelContratosCSV> registros;
                using (var stream = new StreamReader(file.OpenReadStream(), Encoding.GetEncoding("iso-8859-1")))
                using (var csvReader = new CsvReader(stream, configuration))
                {
                    csvReader.Read();
                    csvReader.ReadHeader();

                    registros = csvReader.GetRecords<ModelContratosCSV>().ToList();

                    // Registrar os registros no banco de dados
                    _context.contratos.AddRange(registros);
                    await _context.SaveChangesAsync();

                }
                var arquivo = new ModelArquivos
                {
                    Nome = file.FileName,
                    EmailUsuario = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value,
                };

                _context.arquivos.AddRange(arquivo);
                await _context.SaveChangesAsync();

                return Ok($"Registros do arquivo CSV registrados no banco de dados com sucesso. Total de registros: {registros.Count}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao processar o arquivo CSV: {ex.Message}");
            }

        }

    }
}
