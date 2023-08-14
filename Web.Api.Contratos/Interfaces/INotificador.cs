using Web.Api.Contratos.Notificacoes;

namespace Web.Api.Contratos.Interfaces
{
    //Interface para obter notificacoes.
    //A mesma é injetada na controller main
    public interface INotificador
    {
        bool TemNotificacao();
        List<Notificacao> ObterNotificacoes();
        void Handle(Notificacao notificacao);
    }
}
