using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Estudos.EFCore.DataAnnotations.Domain
{
    [Table("TabelaAtributos")]
    public class Atributo
    {
        [Key]
        public int Id { get; set; }
        [Column("MinhaDescricao", TypeName = "VARCHAR(100)")]
        public string Descricao { get; set; }

        [Required]
        [MaxLength(255)]
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