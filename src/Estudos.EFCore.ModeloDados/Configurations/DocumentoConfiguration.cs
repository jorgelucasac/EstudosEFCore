using Estudos.EFCore.ModeloDados.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estudos.EFCore.ModeloDados.Configurations
{
    public class DocumentoConfiguration : IEntityTypeConfiguration<Documento>
    {
        public void Configure(EntityTypeBuilder<Documento> builder)
        {
            //builder.Property(p => p.CPF)
            //    .HasField("_cpf");

            //builder.Property("_cpf");

            builder
                .Property("_cpf")
                .HasColumnName("CPF")
                .HasMaxLength(11);
        }
    }
}