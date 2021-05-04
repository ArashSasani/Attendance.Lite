using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using WebApplication.SharedDatabase.Logging;

namespace WebApplication.SharedDatabase
{
    public class CustomDbConfig : DbConfiguration
    {
        public CustomDbConfig()
        {
            //SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());

            //interception
            DbInterception.Add(new DbInterceptorLogging());
            SetDatabaseLogFormatter((context, writeAction) => new OneLineFormatter(context, writeAction));
        }
    }
}
