using System;
using System.Linq;
using System.Threading.Tasks;
using Estudos.EFCore.Interceptadores.Data;
using Estudos.EFCore.Interceptadores.Domain;
using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.Interceptadores
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //await TesteInterceptacao();
            await TesteInterceptacaoSaveChanges();
        }

        static async Task TesteInterceptacao()
        {
            using (var db = new ApplicationDbContext())
            {
                var funcaos = db.Funcoes.TagWith("Use NOLOCK").ToList();
                foreach (var funcao in funcaos)
                {
                    Console.WriteLine(funcao.Descricao1);
                }
            }
        }

        static async Task TesteInterceptacaoSaveChanges()
        {
            await using var db = new ApplicationDbContext();
            await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();

            db.Funcoes.Add(new Funcao
            {
                Descricao1 = "Teste"
            });

            await db.SaveChangesAsync();
        }

    }
}
