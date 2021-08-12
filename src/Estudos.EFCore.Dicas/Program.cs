using System;
using System.Linq;
using Estudos.EFCore.Dicas.Data;
using Estudos.EFCore.Dicas.Domain;
using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.Dicas
{
    class Program
    {
        static void Main(string[] args)
        {
            //ToQueryString();
            //DebugView();
            Clear();
        }

        /// <summary>
        /// retorna a sql que será executada, mas não executa
        /// </summary>
        static void ToQueryString()
        {
            using var db = new ApplicationContext();
            db.Database.EnsureCreated();

            var query = db.Departamentos.Where(p => p.Id > 2);

            var sql = query.ToQueryString();

            Console.WriteLine(sql);
        }

        /// <summary>
        /// DebugView é uma propriedade que pode ser analisada em modo debug
        /// </summary>
        static void DebugView()
        {
            using var db = new ApplicationContext();

            //db.ChangeTracker.DebugView;
            
            db.Departamentos.Add(new Departamento { Descricao = "TESTE DebugView" });


            var query = db.Departamentos.Where(p => p.Id > 2);

            //somente em debug
            //query.DebugView
        }

        /// <summary>
        /// Redefinindo o estado do contexto
        /// </summary>
        static void Clear()
        {
            using var db = new ApplicationContext();

            db.Departamentos.Add(new Departamento { Descricao = "TESTE DebugView" });

            //limpar o track
            //descartando as entidades que estão sendo rastreadas
            db.ChangeTracker.Clear();
        }
    }
}
