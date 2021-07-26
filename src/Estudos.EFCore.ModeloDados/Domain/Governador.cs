namespace Estudos.EFCore.ModeloDados.Domain
{
    public class Governador
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public string Partido { get; set; }

        public int EstadoReference { get; set; }
        public Estado Estado { get; set; }
    }
}