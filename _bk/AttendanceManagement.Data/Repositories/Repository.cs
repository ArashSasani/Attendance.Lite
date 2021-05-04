using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using WebApplication.Infrastructure.Interfaces;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>, IDisposable
        where TEntity : class, IEntity
    {
        private readonly CrudContext _context;
        private readonly DbSet<TEntity> _dbSet;

        private readonly IExceptionLogger _logger;

        public Repository(CrudContext context, IExceptionLogger logger)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();

            _logger = logger;
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null
            , Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null
            , string includeProperties = "", bool trackByTheContext = true)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            //filter soft deleted records
            query = query.Where(q => q.DeleteState != DeleteState.SoftDelete);

            //for using ef eager loading
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (!trackByTheContext)
            {
                return query.AsNoTracking();
            }
            else
            {
                return query;
            }
        }

        public TEntity GetById(int id, bool trackByTheContext = true)
        {
            if (!trackByTheContext)
            {
                return _dbSet.AsNoTracking().SingleOrDefault(q => q.Id == id
                && q.DeleteState != DeleteState.SoftDelete);
            }
            return _dbSet.SingleOrDefault(q => q.Id == id
                && q.DeleteState != DeleteState.SoftDelete);
        }

        public bool Exists(int id)
        {
            return _dbSet.Any(q => q.Id == id && q.DeleteState != DeleteState.SoftDelete);
        }

        public void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
            Commit();
        }

        public void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            Commit();
        }

        public void Delete(int id, DeleteState deleteState)
        {
            var entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete, deleteState);
        }

        public void Delete(TEntity entity, DeleteState deleteState)
        {
            switch (deleteState)
            {
                case DeleteState.SoftDelete:
                    SoftDelete(entity);
                    break;
                case DeleteState.Permanent:
                    PermanentDelete(entity);
                    break;
                default:
                    break;
            }

        }

        #region delete state
        private void SoftDelete(TEntity entity)
        {
            entity.DeleteState = DeleteState.SoftDelete;
            Update(entity);
        }

        private void PermanentDelete(TEntity entity)
        {
            _dbSet.Remove(entity);
            Commit();
        }
        #endregion

        public IEnumerable<TEntity> ExecQuery(string query, bool trackByTheContext = true
            , params object[] parameters)
        {
            var result = _dbSet.SqlQuery(query, parameters);
            if (!trackByTheContext)
                result.AsNoTracking().AsEnumerable();
            return result.AsEnumerable();
        }

        private void Commit()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogRunTimeError(ex, "exception message: {0}, inner exception message: {1}"
                    , ex.Message, ex.InnerException != null
                    ? ex.InnerException.Message : "[empty]");
                throw;
            }
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
