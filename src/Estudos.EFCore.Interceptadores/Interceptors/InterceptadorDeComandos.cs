using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Estudos.EFCore.Interceptadores.Interceptors
{
    public class InterceptadorDeComandos : DbCommandInterceptor
    {
        private static readonly Regex TableRegex =
            new Regex(@"(?<tableAlias>FROM +(\[.*\]\.)?(\[.*\]) AS (\[.*\])(?! WITH \(NOLOCK\)))",
                RegexOptions.Multiline |
                RegexOptions.IgnoreCase |
                RegexOptions.Compiled);
        public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        {
            Console.WriteLine("Entrando no método ReaderExecuting");

            UsarNoLock(command);

            return result;
        }

        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            Console.WriteLine("Entrando no método ReaderExecutingAsync");


            UsarNoLock(command);

            return new ValueTask<InterceptionResult<DbDataReader>>(result);

        }

        private static void UsarNoLock(DbCommand command)
        {
            if (!command.CommandText.Contains("WITH (NOLOCK)")
                && command.CommandText.StartsWith("-- Use NOLOCK"))
            {
                command.CommandText = TableRegex.Replace(command.CommandText, "${tableAlias} WITH (NOLOCK)");
            }
        }
    }
}
