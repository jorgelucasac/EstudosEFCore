using System;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Estudos.EFCore.Interceptadores.Interceptors
{
    public class InterceptadorDeConexao : DbConnectionInterceptor
    {
        public override InterceptionResult ConnectionOpening(
            DbConnection connection,
            ConnectionEventData eventData,
            InterceptionResult result)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Entrei no metodo ConnectionOpening");

            var connectionString = ((SqlConnection)connection).ConnectionString;

            Console.WriteLine(connectionString);

            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString)
            {
                //DataSource="IP Segundo Servidor",
                ApplicationName = "CursoEFCore"
            };

            connection.ConnectionString = connectionStringBuilder.ToString();

            Console.WriteLine(connectionStringBuilder.ToString());
            Console.ResetColor();
            return result;
        }
    }
}