using System.Data.Entity;

namespace WebApplication.SharedDatabase.EF
{
    public class BaseContext<TContext> : DbContext
            where TContext : DbContext
    {
        static BaseContext()
        {
            //disable initialization for other contexts
            Database.SetInitializer<TContext>(null);
        }

        protected BaseContext() : base("AttendanceLite")
        {
        }
    }
}
