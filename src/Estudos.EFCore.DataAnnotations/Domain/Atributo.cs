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
        public string Observação { get; set; }

    }
}