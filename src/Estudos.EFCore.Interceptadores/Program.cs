using System;
using System.Linq;
using System.Threading.Tasks;
using Estudos.EFCore.Interceptadores.Data;
using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.Interceptadores
{
    class Program
    {
        static async Task Main(string[] args)
        {
           await TesteInterceptacao();
        }

        static async Task TesteInterceptacao()
        {
            using (var db = new ApplicationDbContext())
            {
                var funcaos = await db.Funcoes.ToListAsync();
                foreach (var funcao in funcaos)
                {
                    Console.WriteLine(funcao.Descricao1);
                }
            }
        }
    }
}
