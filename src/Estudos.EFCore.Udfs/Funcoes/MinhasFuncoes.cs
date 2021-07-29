using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Estudos.EFCore.Udfs.Funcoes
{
    public class MinhasFuncoes
    {
        //com esse atributo é possível traduzuzir o método para uma função do BD
        //assim, ao chamar o método o EFCore vai chamar a função do banco
        [DbFunction(name: "LEFT", IsBuiltIn = true)]
        public static string Left(string dados, int quantidade)
        {
            throw new NotImplementedException();
        }



        public static string LetrasMaiusculas(string dados)
        {
            throw new NotImplementedException();
        }

        public static int DateDiff(string identificador, DateTime dataInicial, DateTime dataFinal)
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
