using System.Collections.Generic;

namespace Estudos.EFCore.Domain
{
    public class Departamento
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public List<Funcionario> Funcionarios { get; set; }
    }
}