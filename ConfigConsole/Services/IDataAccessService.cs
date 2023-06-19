using ConfigConsole.Models;

namespace ConfigConsole.Services
{
    public interface IDataAccessService
    {
        Task<ICollection<PurgeModel>> CreatePurgeData();
        Task<ICollection<T>> GetAll<T>();
        Task<T> GetOne<T>(string id) where T : ILocalStorageModel, new();
        Task InitializeData();
        Task Remove<T>(string id) where T : ILocalStorageModel;
        Task Upsert<T>(T record) where T : ILocalStorageModel;
    }
}