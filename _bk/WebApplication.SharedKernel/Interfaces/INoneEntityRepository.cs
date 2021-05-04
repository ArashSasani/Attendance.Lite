using System.Linq;

namespace WebApplication.SharedKernel.Interfaces
{
    /// <summary>
    /// encapsulates direct command and query methods for underlying database
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface INoneEntityRepository<T>
    {
        /// <summary>
        /// execute query against specified entity or none-entity types in the database.
        /// the results will not track by the context.
        /// </summary>
        /// <param name="query">sql query to execute</param>
        /// <param name="parameters">any parameter values you supply will automatically be 
        /// converted to a DbParameter to protect against sql injection</param>
        /// <returns></returns>
        IQueryable<T> ExecQueryForAll(string query, params object[] parameters);

        /// <summary>
        /// execute query against specified entity or none-entity types in the database.
        /// the results will not track by the context.
        /// </summary>
        /// <param name="query">sql query to execute</param>
        /// <param name="parameters">any parameter values you supply will automatically be 
        /// converted to a DbParameter to protect against sql injection</param>
        /// <returns></returns>
        T ExecQueryForSingle(string query, params object[] parameters);

        /// <summary>
        /// execute command against the database
        /// </summary>
        /// <param name="query">sql command to execute</param>
        /// <param name="parameters">any parameter values you supply will automatically be 
        /// converted to a DbParameter to protect against sql injection</param>
        /// <returns>rows affected</returns>
        int ExecCommand(string sql, params object[] parameters);

        /// <summary>
        /// execute commands as transaction against the database
        /// </summary>
        /// <param name="query">sql command to execute</param>
        /// <param name="parameters">any parameter values you supply will automatically be 
        /// converted to a DbParameter to protect against sql injection</param>
        void ExecCommandAsTransaction(string sql, params object[] parameters);
    }
}
