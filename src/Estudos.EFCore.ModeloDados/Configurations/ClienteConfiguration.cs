using Estudos.EFCore.ModeloDados.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estudos.EFCore.ModeloDados.Configurations
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            //informa que o endereço não é uma tabela
            //e que os campos da entidade endereço devem ser
            //criados na tabela cliente

            //forma 1
            //c.OwnsOne(x => x.Endereco);

            //forma 2 - informa o nome das colunas
            builder.OwnsOne(x => x.Endereco, and =>
                {
                    and.Property(p => p.Estado).HasColumnName("Estado");
                    and.Property(p => p.Cidade).HasColumnName("Cidade");
                    and.Property(p => p.Bairro).HasColumnName("Bairro");
                    and.Property(p => p.Logradouro).HasColumnName("Logradouro");

                    //cria uma tebela para o endereco - OPCIONAL
                    and.ToTable("Endereco");
                });
        }
    }
}