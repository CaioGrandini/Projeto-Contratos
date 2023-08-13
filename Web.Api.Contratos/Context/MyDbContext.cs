using Microsoft.EntityFrameworkCore;
using Web.Api.Contratos.Models;

namespace Web.Api.Contratos.Context
{
    public class MyDbContext : DbContext
    {
        //Configuracao do contexto no banco de dados.
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = true;
        }

       public DbSet<ModelContratosCSV> contratos { get; set; }
       public DbSet<ModelArquivos> arquivos { get; set; }
    }
}
