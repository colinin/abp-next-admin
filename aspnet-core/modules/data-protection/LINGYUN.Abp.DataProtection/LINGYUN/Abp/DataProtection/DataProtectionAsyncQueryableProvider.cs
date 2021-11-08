using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Linq;

namespace LINGYUN.Abp.DataProtection
{
    public class DataProtectionAsyncQueryableProvider : IAsyncQueryableProvider
    {
        public Task<bool> AllAsync<T>(
            IQueryable<T> queryable, 
            Expression<Func<T, bool>> predicate, 
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> AverageAsync(IQueryable<decimal> source, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<decimal?> AverageAsync(IQueryable<decimal?> source, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, decimal>> selector, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<decimal?> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<double> AverageAsync(IQueryable<int> source, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<double?> AverageAsync(IQueryable<int?> source, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<double> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, int>> selector, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<double?> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, int?>> selector, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<double> AverageAsync(IQueryable<long> source, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<double?> AverageAsync(IQueryable<long?> source, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<double> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, long>> selector, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<double?> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, long?>> selector, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<double> AverageAsync(IQueryable<double> source, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<double?> AverageAsync(IQueryable<double?> source, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<double> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, double>> selector, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<double?> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, double?>> selector, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<float> AverageAsync(IQueryable<float> source, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<float?> AverageAsync(IQueryable<float?> source, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<float> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, float>> selector, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<float?> AverageAsync<T>(IQueryable<T> queryable, Expression<Func<T, float?>> selector, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public bool CanExecute<T>(IQueryable<T> queryable)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ContainsAsync<T>(IQueryable<T> queryable, T item, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> FirstAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> FirstAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> LastAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> LastAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> LastOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> LastOrDefaultAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<long> LongCountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<long> LongCountAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> MaxAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> MaxAsync<T, TResult>(IQueryable<T> queryable, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> MinAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> MinAsync<T, TResult>(IQueryable<T> queryable, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> SingleAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> SingleAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> SingleOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> SingleOrDefaultAsync<T>(IQueryable<T> queryable, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> SumAsync(IQueryable<decimal> source, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<decimal?> SumAsync(IQueryable<decimal?> source, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, decimal>> selector, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<decimal?> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, decimal?>> selector, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> SumAsync(IQueryable<int> source, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int?> SumAsync(IQueryable<int?> source, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, int>> selector, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int?> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, int?>> selector, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<long> SumAsync(IQueryable<long> source, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<long?> SumAsync(IQueryable<long?> source, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<long> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, long>> selector, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<long?> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, long?>> selector, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<double> SumAsync(IQueryable<double> source, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<double?> SumAsync(IQueryable<double?> source, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<double> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, double>> selector, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<double?> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, double?>> selector, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<float> SumAsync(IQueryable<float> source, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<float?> SumAsync(IQueryable<float?> source, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<float> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, float>> selector, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<float?> SumAsync<T>(IQueryable<T> queryable, Expression<Func<T, float?>> selector, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T[]> ToArrayAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> ToListAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
