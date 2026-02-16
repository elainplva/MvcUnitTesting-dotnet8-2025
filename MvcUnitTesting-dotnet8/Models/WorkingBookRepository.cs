using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MvcUnitTesting_dotnet8.Models
{
    public class WorkingBookRepository<T> : IRepository<T> where T : class
    {
        protected readonly BookDbContext Context;
        public WorkingBookRepository(BookDbContext context)
        {
            Context = context;
        }
        public T Get(int id)
        {
            return Context.Set<T>().Find(id);
        }
        //public List<T> GetAll()
        //{
        //    return Context.Set<T>().ToList();

        //}

        public void Add(T entity)
        {
            Context.Set<T>().Add(entity);
            Context.SaveChanges();
        }

        public void Update(T entity)
        {
            Context.Set<T>().Update(entity);
            Context.SaveChanges();
        }

        public void Remove(T entity)
        {
            Context.Set<T>().Remove(entity);
            Context.SaveChanges();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return Context.Set<T>().Where(predicate);
        }


        IEnumerable<T> IRepository<T>.GetAll()
        {
            return Context.Set<T>().ToList();

        }


        public void AddRange(IEnumerable<T> entities)
        {
            Context.Set<T>().AddRange(entities);
            Context.SaveChanges();
        }


        public void RemoveRange(IEnumerable<T> entities)
        {
            Context.Set<T>().RemoveRange(entities);
            Context.SaveChanges();
        }
    }
}