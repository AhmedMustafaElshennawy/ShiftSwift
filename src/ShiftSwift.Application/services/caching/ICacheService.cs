namespace ShiftSwift.Application.services.caching
{
    public interface ICacheService
    {
        Task SetAsync<T>(string key, T value, TimeSpan expiration);
        Task<T> GetAsync<T>(string key);
    }
}
