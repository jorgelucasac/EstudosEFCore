using Estudos.EFCore.ModeloDados.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estudos.EFCore.ModeloDados.Configurations
{
    /// <summary>
    /// Configuração TPH (Tabela Por Hierarquia)
    ///
    /// cria um unica tabela com os campos da classe base das subclasses 
    /// </summary>
    //public class PessoaConfiguration : IEntityTypeConfiguration<Pessoa>
    //{
    //    public void Configure(EntityTypeBuilder<Pessoa> builder)
    //    {
    //        builder.ToTable("Pessoas")
    //            //informa o nome do campo que possui o tipo de cada registro
    //            .HasDiscriminator<int>("TipoPessoa")
    //            //informa os valores que representam classe
    //            .HasValue<Pessoa>(10)
    //            .HasValue<Instrutor>(20)
    //            .HasValue<Aluno>(30);
    //    }
    //}

    /// <summary>
    /// Configuração TPT (Tabela Por Tipo)
    ///
    /// cada classe representa uma tabela no banco de dados
    /// </summary>
    public class PessoaConfiguration : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder.ToTable("Pessoas");
        }
    }

    public class InstrutorConfiguration : IEntityTypeConfiguration<Instrutor>
    {
        public void Configure(EntityTypeBuilder<Instrutor> builder)
        {
            builder.ToTable("Instrutores");
        }
    }

    public class AlunoConfiguration : IEntityTypeConfiguration<Aluno>
    {
        public void Configure(EntityTypeBuilder<Aluno> builder)
        {
            builder.ToTable("Alunos");
        }
    }
}