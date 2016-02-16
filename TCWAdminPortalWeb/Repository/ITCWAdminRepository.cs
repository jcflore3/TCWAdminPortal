using System;
using System.Collections.Generic;

namespace TCWAdminPortalWeb.Repository
{
    /// <summary>
    /// Interface for Generic Repository to be used with all TCWAdmin objects
    /// to get and modify data in the database.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITCWAdminRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(object Id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(object Id);
        void Save();
    }
}
