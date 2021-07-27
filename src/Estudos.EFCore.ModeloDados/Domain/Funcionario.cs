namespace Estudos.EFCore.ModeloDados.Domain
{
    public class Funcionario
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public string CPF { get; set; }
        public string RG { get; set; }
        public bool Excluido { get; set; }

        //sera criada como propriedade de sombra
        //public int DepartamentoId { get; set; }
        public Departamento Departamento { get; set; }
    }
}