using Microsoft.Extensions.Configuration;

namespace Estudos.EFCore.Functions.Helper
{
    public sealed class ConfigurationHelper
    {
        private readonly IConfiguration _config;
        private static ConfigurationHelper _instancia;
        private static readonly object Locker = new object();

        private ConfigurationHelper()
        {
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public static IConfiguration ObterConfiguration()
        {

            if (_instancia == null)
            {
                lock (Locker)
                {
                    if (_instancia == null)
                    {
                        _instancia = new ConfigurationHelper();
                    }
                }
            }


            return _instancia._config;
        }
    }
}