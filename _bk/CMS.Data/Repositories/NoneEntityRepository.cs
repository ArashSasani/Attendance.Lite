using CMS.Data.Identity;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using WebApplication.SharedKernel.Interfaces;

namespace CMS.Data.Repositories
{
    public class NoneEntityRepository<T> : INoneEntityRepository<T>, IDisposable
    {
        private readonly AuthContext _context;

        public NoneEntityRepository(AuthContext context)
        {
            _context = context;
        }

        public int ExecCommand(string sql, params object[] parameters)
        {
            return _context.Database.ExecuteSqlCommand(sql, parameters);
        }

        public void ExecCommandAsTransaction(string sql, params object[] parameters)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    ExecCommand(sql, parameters);

                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }

        public IQueryable<T> ExecQueryForAll(string query, params object[] parameters)
        {
            return ExecQuery(query, parameters).AsQueryable();
        }

        public T ExecQueryForSingle(string query, params object[] parameters)
        {
            return ExecQuery(query, parameters).SingleOrDefault();
        }

        private DbRawSqlQuery<T> ExecQuery(string query, params object[] parameters)
        {
            return _context.Database.SqlQuery<T>(query, parameters);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
