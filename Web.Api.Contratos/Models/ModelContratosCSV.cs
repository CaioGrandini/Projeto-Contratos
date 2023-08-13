using CsvHelper.Configuration;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Api.Contratos.Models
{
    public class ModelContratosCSV
    {
        public string Nome { get; set; }
        public string CPF { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Contrato { get; set; }
        public string Produto { get; set; }
        public string Vencimento { get; set; }
        public float Valor { get; set; }
    }
}
