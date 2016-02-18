using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

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
        Task<CloudBlockBlob> UploadAndSaveBlobAsync(HttpPostedFileBase imageFile);
        Task DeleteBlobsAsync(string imageURL, string thumbnailURL);
        Task DeleteBlobAsync(Uri blobUri);
        Task<CloudBlockBlob> InsertImageBlob(HttpPostedFileBase imageFile);
        Task AddMessageToQueue(string IdString, string objTypeStr);
        void Save();
        Task<int> SaveAsync();
    }
}
