using System.Net;

namespace Estudos.EFCore.ModeloDados.Domain
{
    public class Conversor
    {
        public int Id { get; set; }
        public bool Ativo { get; set; }
        public bool Excluido { get; set; }
        public Versao Versao { get; set; }
        // ReSharper disable once InconsistentNaming
        public IPAddress EnderecoIP { get; set; }
    }

    public enum Versao
    {
        EfCore1,
        EfCore2,
        EfCore3,
        EfCore5,
    }
}