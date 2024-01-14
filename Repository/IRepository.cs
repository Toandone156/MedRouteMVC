using MedRoute.Models.System;

namespace MedRoute.Repository
{
    public interface IRepository<T>
    {
        Task<StatusMessage> GetAllAsync();
        Task<StatusMessage> GetByIdAsync(int id);
        Task<StatusMessage> GetByConditionsAsync(Predicate<T> condition);
        Task<StatusMessage> CreateAsync(T entity);
        Task<StatusMessage> UpdateAsync(T entity);
        Task<StatusMessage> DeleteAsync(int id);
    }
}
