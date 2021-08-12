using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace Estudos.EFCore.SobrescrevendoComportamentos
{
    public class MySqlServerQuerySqlGeneratorFactory : SqlServerQuerySqlGeneratorFactory
    {

        private readonly QuerySqlGeneratorDependencies _dependencies;

        public MySqlServerQuerySqlGeneratorFactory(QuerySqlGeneratorDependencies dependencies) : base(dependencies)
        {
            _dependencies = dependencies;
        }

        public override QuerySqlGenerator Create()
        {
            return new MySqlServerQuerySqlGenerator(_dependencies);
        }
    }
}