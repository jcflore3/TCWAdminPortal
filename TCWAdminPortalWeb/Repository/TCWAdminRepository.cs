using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TCWAdminPortalWeb.Models;

namespace TCWAdminPortalWeb.Repository
{
    /// <summary>
    /// Generic Repository, that implements the ITCWAdminRepository, to be used with all 
    /// TCWAdmin objects to get and modify data in the database.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TCWAdminRepository<T> : ITCWAdminRepository<T> where T : class
    {

        private TCWAdminContext dbContext;
        private DbSet<T> dbSet;

        public TCWAdminRepository()
        {
            dbContext = new TCWAdminContext();
            dbSet = dbContext.Set<T>();
        }

        public void Delete(object Id)
        {
            T getObjById = dbSet.Find(Id);
            dbSet.Remove(getObjById);
        }

        public IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public T GetFirst()
        {
            return dbSet.First();
        }

        public T GetById(object Id)
        {
            return dbSet.Find(Id);
        }

        public void Insert(T obj)
        {
            dbSet.Add(obj);
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public void Update(T obj)
        {
            dbContext.Entry(obj).State = EntityState.Modified;
        }
    }
}
