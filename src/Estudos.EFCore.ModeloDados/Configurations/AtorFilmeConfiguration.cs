using System;
using System.Collections.Generic;
using Estudos.EFCore.ModeloDados.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estudos.EFCore.ModeloDados.Configurations
{
    public class AtorFilmeConfiguration : IEntityTypeConfiguration<Ator>
    {
        public void Configure(EntityTypeBuilder<Ator> builder)
        {
            //forma simples
            //builder
            //    .HasMany(p => p.Filmes)
            //    .WithMany(p => p.Atores)
            //    .UsingEntity(p => p.ToTable("AtoresFilmes"));

            builder
                .HasMany(p => p.Filmes)
                .WithMany(p => p.Atores)
                //nome da tabela e objeto de configurações
                .UsingEntity<Dictionary<string, object>>(
                    //nome da tabela
                    "FilmesAtores",

                    //informa que filmes possui varios atoes e nomeia a chave estrangeira
                    p => p.HasOne<Filme>().WithMany().HasForeignKey("FilmeId"),
                    //informa que ator possui varios filmes e nomeia a chave estrangeira
                    p => p.HasOne<Ator>().WithMany().HasForeignKey("AtorId"),
                    p =>
                    {
                        //cria uma propriedade de sombra
                        p.Property<DateTime>("CadastradoEm").HasDefaultValueSql("GETDATE()");
                    }
                );
        }
    }
}