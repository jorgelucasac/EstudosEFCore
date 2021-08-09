using Estudos.EFCore.MultiTenant.Domain.Abstract;

namespace Estudos.EFCore.MultiTenant.Domain
{
    public class Pessoa : BaseEntity
    {
        public string Nome { get; set; }
    }
}