using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Estudos.EFCore.SobrescrevendoComportamentos
{
    public class MyInterceptor : IObserver<KeyValuePair<string, object>>
    {
        private static readonly Regex TableAliasRegex =
            new Regex(@"(?<tableAlias>FROM +(\[.*\]\.)?(\[.*\]) AS (\[.*\])(?! WITH \(NOLOCK\)))",
                RegexOptions.Multiline | 
                RegexOptions.IgnoreCase | 
                RegexOptions.Compiled);
                
        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(KeyValuePair<string, object> value)
        {
            if(value.Key == RelationalEventId.CommandExecuting.Name)
            {
                var command = ((CommandEventData)value.Value).Command;

                if(!command.CommandText.Contains("WITH (NOLOCK)"))
                {
                    command.CommandText = TableAliasRegex.Replace(command.CommandText,"${tableAlias} WITH (NOLOCK)" );

                    Console.WriteLine(command.CommandText);
                }
            }
        }
    }

    public class MyInterceptorListener : IObserver<DiagnosticListener>
    {
        private readonly MyInterceptor _interceptor = new MyInterceptor();
                
        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(DiagnosticListener listener)
        {
            if(listener.Name == DbLoggerCategory.Name)
            {
                listener.Subscribe(_interceptor);
            }
        }
    }
}