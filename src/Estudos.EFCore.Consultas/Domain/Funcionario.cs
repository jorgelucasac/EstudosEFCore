namespace Estudos.EFCore.Consultas.Domain
{
    public class Funcionario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        // ReSharper disable once InconsistentNaming
        public string CPF { get; set; }
        // ReSharper disable once InconsistentNaming
        public string RG { get; set; }
        public int DepartamentoId { get; set; }
        public virtual Departamento Departamento { get; set; }
    }
}