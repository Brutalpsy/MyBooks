using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MyBooks.Interface.Repositories;
using SQLite;

namespace MyBooks.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private SQLiteAsyncConnection _dbConnection;
        public Repository(SQLiteAsyncConnection dbConnection)
        {
            _dbConnection = dbConnection;
            CreateTableIfNotExists();
        }

        public AsyncTableQuery<T> AsQueryable()
        {
            return _dbConnection.Table<T>();
        }

        public async Task<bool> Delete(T entity)
        {
            return IsSuccess(await _dbConnection.DeleteAsync(entity));
        }

        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await _dbConnection.Table<T>().Where(predicate).ToListAsync();
        }

        public async Task<T> Get(int id)
        {
            return await _dbConnection.FindAsync<T>(id);
        }

        public async Task<T> Get(Expression<Func<T, bool>> predicate)
        {
            return await _dbConnection.FindAsync<T>(predicate);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbConnection.Table<T>().ToListAsync();
        }

        public async Task<bool> Insert(T entity)
        {
            return IsSuccess(await _dbConnection.InsertAsync(entity));
        }

        public async Task<bool> Insert(IEnumerable<T> items)
        {
            return IsSuccess(await _dbConnection.InsertAllAsync(items, true));
        }

        public async Task<bool> Update(T entity)
        {
            return IsSuccess(await _dbConnection.InsertOrReplaceAsync(entity));

        }
        private bool IsSuccess(int rowsAffected)
        {
            return rowsAffected > 0;
        }
        private async void CreateTableIfNotExists()
        {
            await _dbConnection.CreateTableAsync<T>();
        }
    }
}
