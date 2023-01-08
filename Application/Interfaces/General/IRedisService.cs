

namespace Application.Interfaces.General
{
    public interface IRedisService
    {
        public void CreateTransaction();
        bool StringSetAsync(string key, object value);
        bool KeyDeleteAsync(string key);
        Task<T?> GetAsync<T>(string key) where T : class;
        Task<bool> ExistAsync(string key);
        Task<bool> ExecuteAsync();

    }
}