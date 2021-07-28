using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.ModeloDados.Domain
{
    public class Documento
    {
        private string _cpf;

        public int Id { get; set; }

        public void SetCpf(string cpf)
        {
            // Validações
            if (string.IsNullOrWhiteSpace(cpf))
            {
                throw new System.Exception("CPF Invalido");
            }

            _cpf = cpf;
        }

        
        [BackingField(nameof(_cpf))]
        public string CPF => _cpf;

    }
}