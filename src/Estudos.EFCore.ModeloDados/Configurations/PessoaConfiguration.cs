using Estudos.EFCore.ModeloDados.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estudos.EFCore.ModeloDados.Configurations
{
    public class PessoaConfiguration : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder.ToTable("Pessoas")
                //informa o nome do campo que possui o tipo de cada registro
                .HasDiscriminator<int>("TipoPessoa")
                //informa os valores que representam classe
                .HasValue<Pessoa>(10)
                .HasValue<Instrutor>(20)
                .HasValue<Aluno>(30);
        }
    }
}