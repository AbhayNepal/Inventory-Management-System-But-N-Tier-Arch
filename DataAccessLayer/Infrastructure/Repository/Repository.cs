using Microsoft.EntityFrameworkCore;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.DataAccessLayer.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
         }

        public void DeleteRange(IEnumerable<T> entity)
        {
            _dbSet.RemoveRange(entity);
        }

        public IEnumerable<T> GetAll(string? includeProperties = null)//include properties contains the names of the tables you want to include in the product table separated by comma
        {
            IQueryable<T> query = _dbSet; //make product dbset queryable
            if(includeProperties != null)
            {
                foreach(var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))//here we are separating multiple table names which are sent separated by comma
                {
                    query = query.Include(item); //each table name received in the includeProperties variable will be included in the product dbset one by one.  
                }                                //while including the items of the new table to the _dbSet i think it maintains the primary key foreign key relation.
            }
            return query.ToList();
        }

        public T GetT(Expression<Func<T, bool>> predicate,string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet; //make product dbset queryable
            query = query.Where(predicate);// jst the difference from GetAll method.
            if (includeProperties != null)
            {
                foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item); //here the includeProperties will receive the values of category table that will be included in the product dbset for the access approval of category table from products controller.
                }
            }

            return query.FirstOrDefault();
        }
    }
}
