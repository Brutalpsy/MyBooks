using SQLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyBooks.Interface.Repositories
{
    public interface IRepository<T> where T : class, new()
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(int id);
        Task<T> Get(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
        AsyncTableQuery<T> AsQueryable();
        Task<bool> Insert(T entity);
        Task<bool> Insert(IEnumerable<T> items);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
    }
}
