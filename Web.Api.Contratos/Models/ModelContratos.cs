using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Api.Contratos.Models
{
    [Table("Produto")]
    public class ModelContratos
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public int Contrato { get; set; }
        public string Produto { get; set; }
        public DateTime Vencimento { get; set; }
        public float Valor { get; set; }
    }
}
