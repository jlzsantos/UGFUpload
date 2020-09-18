using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using UGFUpload.Repository.Data;
using UGFUpload.Repository.Entities;

namespace UGFUpload.Repository.Respositories
{
    public class BaseRepository<T> where T : EntityBase
    {
        private readonly ApplicationDbContext _db;

        public BaseRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool Create(T entity)
        {
            entity.CreatedDate = DateTime.Now;
            _db.Add<T>(entity);
            return Save();
        }

        public bool Delete(int id)
        {
            var entity = FindById(id);

            if (entity == null)
                throw new Exception("Entity not found.");

            _db.Remove<T>(entity);
            return Save();
        }

        public ICollection<T> FindAll(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _db.Set<T>();
            query = includes.Aggregate(query, (current, include) => current.Include(include));
            var list = query.ToList();

            return list;
        }

        public T FindById(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _db.Set<T>();
            query = includes.Aggregate(query, (current, include) => current.Include(include));

            return query.FirstOrDefault(x => x.Id == id);
        }

        public bool IsExists(int id)
        {
            var entity = FindById(id);
            return (entity != null);
        }

        public bool Save()
        {
            return (_db.SaveChanges() > 0);
        }

        public bool Update(T entity)
        {
            if (!IsExists(entity.Id))
                throw new Exception("Entity not found.");

            entity.UpdatedDate = DateTime.Now;

            if (_db.Entry(entity).State == Microsoft.EntityFrameworkCore.EntityState.Detached)
            {
                var oldEntity = FindById(entity.Id);
                entity.CreatedDate = oldEntity.CreatedDate;
                _db.Entry(oldEntity).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }

            _db.Update<T>(entity);
            return Save();
        }
    }
}
