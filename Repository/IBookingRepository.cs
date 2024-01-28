using MedRoute.Models;
using MedRoute.Models.System;

namespace MedRoute.Repository
{
    public interface IBookingRepository : IRepository<Booking>
    {
        public Task<StatusMessage> CreateAsync(Booking entity);
        public Task<StatusMessage> DeleteAsync(int id);
        public Task<StatusMessage> GetAllAsync();
        public Task<StatusMessage> GetByConditionsAsync(Predicate<Booking> condition);
        public Task<StatusMessage> GetByIdAsync(int id);
        public Task<StatusMessage> UpdateAsync(Booking entity);
    }
}
