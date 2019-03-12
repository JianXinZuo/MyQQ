using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WalletComponent.Repositorys
{
    public interface IRepository<T, PK> where T : class
    {
        void Add(T entity);
        void Remove(T entity);
        T Load(PK primaryKey);
        Task<T> LoadAsync(PK primaryKey);
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        IEnumerable<T> FindAll();
        void SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
