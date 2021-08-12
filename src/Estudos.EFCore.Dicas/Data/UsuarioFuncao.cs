using System;
using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.Dicas.Data
{
    [Keyless]//sem chave primaria
    public class UsuarioFuncao
    {
        public Guid UsuarioId { get; set; }
        public Guid FuncaoId { get; set; }
    }
}