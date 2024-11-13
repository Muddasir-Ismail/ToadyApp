using eTickets.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace eTickets.wwwroot.json.Base
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        private readonly AppDbContext _context;

        public EntityBaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return query.ToList();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().FirstOrDefault(n => n.Id == id);
        }

        public T GetById(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return query.FirstOrDefault(n => n.Id == id);
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public void Update(int id, T entity)
        {
            var existingEntity = _context.Set<T>().Find(id);
            if (existingEntity != null)
            {
                EntityEntry entityEntry = _context.Entry(existingEntity);
                entityEntry.CurrentValues.SetValues(entity);
                entityEntry.State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var entity = _context.Set<T>().FirstOrDefault(n => n.Id == id);
            if (entity != null)
            {
                EntityEntry entityEntry = _context.Entry(entity);
                entityEntry.State = EntityState.Deleted;
                _context.SaveChanges();
            }
        }
    }
}
