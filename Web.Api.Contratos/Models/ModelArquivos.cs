using System.ComponentModel.DataAnnotations;

namespace Web.Api.Contratos.Models
{
    public class ModelArquivos
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string EmailUsuario { get; set; }
    }
}
