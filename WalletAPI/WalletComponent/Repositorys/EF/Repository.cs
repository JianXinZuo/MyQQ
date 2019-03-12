using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace WalletComponent.Repositorys.EF
{
    public class Repository<T, PK> : IRepository<T, PK> where T : class
    {
        public MyDbContext DbContext { get; set; }

        public void Add(T entity)
        {
            DbContext.Add(entity);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return DbContext.Set<T>().Where(expression).ToList();
        }

        public IEnumerable<T> FindAll()
        {
            return DbContext.Set<T>().ToList();
        }

        public T Load(PK primaryKey)
        {
            return DbContext.Set<T>().Find(new object[] { primaryKey });
        }

        public Task<T> LoadAsync(PK primaryKey)
        {
            return DbContext.Set<T>().FindAsync(new object[] { primaryKey });
        }

        public void Remove(T entity)
        {
            DbContext.Remove(entity);
        }

        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return DbContext.SaveChangesAsync();
        }

        
    }
}
