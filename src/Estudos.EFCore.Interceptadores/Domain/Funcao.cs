using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Estudos.EFCore.Interceptadores.Domain
{
    public class Funcao : IAuditEntity
    {
        public int Id { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        public string Descricao1 { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        public string Descricao2 { get; set; }

        public DateTime Data1 { get; set; }
        public string Data2 { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}