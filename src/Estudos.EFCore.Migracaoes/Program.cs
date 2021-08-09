using System;
using System.Threading.Tasks;
using Estudos.EFCore.Migracoes.Data;
using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.Migracoes
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await using var db = new ApplicationDbContext();

            var migracoes = await db.Database.GetPendingMigrationsAsync();
            foreach (var migracao in migracoes)
            {
                Console.WriteLine(migracao);
            }

        }
    }
}
