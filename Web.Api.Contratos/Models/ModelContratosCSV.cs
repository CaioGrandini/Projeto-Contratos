using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Api.Contratos.Models
{
    public class ModelContratosCSV
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        [Key]
        public int Contrato { get; set; }
        public string Produto { get; set; }
        public DateTime Vencimento { get; set; }
        public float Valor { get; set; }
    }
}
