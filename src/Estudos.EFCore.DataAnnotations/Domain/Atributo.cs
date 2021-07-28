using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.DataAnnotations.Domain
{
    [Table("TabelaAtributos")]
    //cria um indice no banco de dados com os campos informados
    [Index(new[] { nameof(Descricao), nameof(Id) }, IsUnique = true)]
    [Comment("Comentário na tabela")]
    public class Atributo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("MinhaDescricao", TypeName = "VARCHAR(100)")]
        [Comment("Comentário na coluna")]
        public string Descricao { get; set; }

        //[Required]
        [MaxLength(255)]
        //remove esse campo das instruções insert e update
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Observacao { get; set; }

    }

    public class Aeroporto
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        [NotMapped]
        public string PropriedadeNaoMapeada { get; set; }

        //informa qual a propriedade em voo esta relacionada com com essa propriedade
        [InverseProperty("AeroportoChegada")]
        public ICollection<Voo> VoosDeChegada { get; set; }

        [InverseProperty("AeroportoPartida")]
        public ICollection<Voo> VoosDePartida { get; set; }
    }

    //Infroma que a classe/entidade não deve ser mapeada para o banco de dados
    [NotMapped]
    public class Voo
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public Aeroporto AeroportoChegada { get; set; }
        public Aeroporto AeroportoPartida { get; set; }

    }
}