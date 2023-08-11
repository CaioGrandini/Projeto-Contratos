namespace Web.Api.Contratos.Extensions
{
    public class AppSettings
    {
        //chave de criptografica
        public string Secret { get; set; }

        //tempo de expiracao em horas
        public int ExpiracaoHoras { get; set; }

        //quem emite
        public string Emissor { get; set; }

        //Urls que o token é validpo
        public string ValidoEm { get; set; }
    }
}
