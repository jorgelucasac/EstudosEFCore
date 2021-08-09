using Estudos.EFCore.MultiTenant.Domain.Abstract;

namespace Estudos.EFCore.MultiTenant.Domain
{
    public class Produto : BaseEntity
    {
        public string Descricao { get; set; }
    }
}