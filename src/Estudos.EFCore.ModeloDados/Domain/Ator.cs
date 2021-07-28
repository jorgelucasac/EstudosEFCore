using System.Collections.Generic;

namespace Estudos.EFCore.ModeloDados.Domain
{
    public class Ator
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public ICollection<Filme> Filmes { get; } = new List<Filme>();
    }
}