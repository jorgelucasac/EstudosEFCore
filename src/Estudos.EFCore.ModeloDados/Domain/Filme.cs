using System.Collections.Generic;

namespace Estudos.EFCore.ModeloDados.Domain
{
    public class Filme
    {
        public int Id { get; set; }
        public string Descricao { get; set; }

        public ICollection<Ator> Atores { get; } = new List<Ator>();
    }
}