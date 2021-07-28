using System;
using System.Linq;
using System.Linq.Expressions;
using Estudos.EFCore.ModeloDados.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Estudos.EFCore.ModeloDados.Conversores
{
    public class ConversorCustomizado : ValueConverter<Status, string>
    {
        public ConversorCustomizado() : base(
            p => ConverterParaOhBancoDeDados(p),
           value => ConverterParaAplicacao(value),
            //tamanho do campo no banco de dados
            new ConverterMappingHints(1))
        {
        }

        /// <summary>
        /// envia apenas o primeiro caractere para o banco
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        static string ConverterParaOhBancoDeDados(Status status)
        {
            return status.ToString()[0..1];
        }

        /// <summary>
        /// pega o balor do banco e identifica qual o valor da enum correspondente
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static Status ConverterParaAplicacao(string value)
        {
            var status = Enum
                .GetValues<Status>()
                .FirstOrDefault(p => p.ToString()[0..1] == value);

            return status;
        }
    }
}