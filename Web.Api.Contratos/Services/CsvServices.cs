using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using Web.Api.Contratos.Context;
using Web.Api.Contratos.Models;

namespace Web.Api.Contratos.Services
{
    public class CsvServices
    {
        private readonly MyDbContext _context;

        public CsvServices(MyDbContext context)
        {
            _context = context;
        }

        public void ImportCsv(string FilePath)
        {
            using (var reader = new StreamReader(FilePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                var records = csv.GetRecords<ModelContratosCSV>().ToList();

                // Salvar os registros no banco de dados
                _context.contratos.AddRange(records);
                _context.SaveChanges();
            }
        }
    }
}
