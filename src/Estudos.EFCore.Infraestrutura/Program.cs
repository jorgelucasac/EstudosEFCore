using System;
using System.Linq;
using Estudos.EFCore.Infraestrutura.Data;

namespace Estudos.EFCore.Infraestrutura
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsultarDepartamentos();
        }

        static void ConsultarDepartamentos()
        {
            using var db = new ApplicationDbContext();

            var departamentos = db.Departamentos.Where(p => p.Id > 0).ToArray();
        }
    }
}
