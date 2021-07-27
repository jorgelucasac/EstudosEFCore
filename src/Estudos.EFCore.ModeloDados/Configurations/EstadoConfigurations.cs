using Estudos.EFCore.ModeloDados.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estudos.EFCore.ModeloDados.Configurations
{
    public class EstadoConfigurations : IEntityTypeConfiguration<Estado>
    {
        public void Configure(EntityTypeBuilder<Estado> builder)
        {
            builder
                //informa que estado tem um governador
                .HasOne(e => e.Governador)
                //informa que governador esta relacionado com o Estado
                .WithOne(g => g.Estado)
                //informa qual a chave estrangeira
                //a entidade que possui a chave estrangeira é a entidade dependente
                .HasForeignKey<Governador>(p => p.EstadoId);
            
            //informa que governador sempre derve ser carregado
            //quando um estado for consultado
            builder.Navigation(e => e.Governador).AutoInclude();
        }
    }
}