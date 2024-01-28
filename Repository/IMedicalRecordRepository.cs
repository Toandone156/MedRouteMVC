using MedRoute.Models;
using MedRoute.Models.System;

namespace MedRoute.Repository
{
    public interface IMedicalRecordRepository : IRepository<MedicalRecord>
    {
        public Task<StatusMessage> CreateAsync(MedicalRecord entity);
        public Task<StatusMessage> DeleteAsync(int id);
        public Task<StatusMessage> GetAllAsync();
        public Task<StatusMessage> GetByConditionsAsync(Predicate<MedicalRecord> condition);
        public Task<StatusMessage> GetByIdAsync(int id);
        public Task<StatusMessage> UpdateAsync(MedicalRecord entity);
    }
}
