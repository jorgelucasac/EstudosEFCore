using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.Udfs.Funcoes
{
    public class MinhasFuncoes
    {
        [DbFunction(name: "LEFT", IsBuiltIn = true)]
        public static string Left(string dados, int quantidade)
        {
            throw new NotImplementedException();
        }

        public static void RegistarFuncoes(ModelBuilder modelBuilder)
        {
            //obtem todos os métodos que possuem o atrinuto DbFunctionAttribute
            var funcoes = typeof(MinhasFuncoes).GetMethods()
                .Where(p => Attribute.IsDefined(p, typeof(DbFunctionAttribute)));

            foreach (var funcao in funcoes)
            {
                modelBuilder.HasDbFunction(funcao);
            }
        }
    }
}
