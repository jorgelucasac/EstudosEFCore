using System;

namespace Estudos.EFCore.Interceptadores.Domain
{
    public interface IAuditEntity
    {
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}