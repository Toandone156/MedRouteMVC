using MedRoute.Database;
using MedRoute.Models;
using MedRoute.Models.System;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MedRoute.Repository.Implement
{
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private readonly AppDBContext _context;
        public MedicalRecordRepository(AppDBContext context)
        {
            this._context = context;
        }

        public async Task<StatusMessage> CreateAsync(MedicalRecord entity)
        {
            try
            {
                if (entity == null)
                {
                    return new StatusMessage()
                    {
                        IsSuccess = false,
                        Message = Message.INPUT_EMPTY
                    };
                }

                await _context.AddAsync<MedicalRecord>(entity);
                await _context.SaveChangesAsync();

                return new StatusMessage()
                {
                    IsSuccess = true,
                    Message = Message.ADD_SUCCESS,
                    Data = entity
                };
            }
            catch (Exception ex)
            {
                return new StatusMessage
                {
                    IsSuccess = false,
                    Message = Message.UNKNOW_ERROR_PREFIX + ex.Message,
                };
            }
        }

        public async Task<StatusMessage> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.MedicalRecords.FirstOrDefaultAsync(c => c.MedicalRecordId == id);

                if (entity == null)
                {
                    return new StatusMessage()
                    {
                        IsSuccess = false,
                        Message = Message.ID_NOT_FOUND
                    };
                }

                _context.MedicalRecords.Remove(entity);
                await _context.SaveChangesAsync();

                return new StatusMessage()
                {
                    IsSuccess = true,
                    Message = Message.DELETE_SUCCESS
                };
            }
            catch (Exception ex)
            {
                return new StatusMessage
                {
                    IsSuccess = false,
                    Message = Message.UNKNOW_ERROR_PREFIX + ex.Message,
                };
            }
        }

        public async Task<StatusMessage> GetAllAsync()
        {
            try
            {
                return new StatusMessage
                {
                    IsSuccess = true,
                    Message = Message.GET_SUCCESS,
                    Data = _context.MedicalRecords
                };
            }
            catch (Exception ex)
            {
                return new StatusMessage
                {
                    IsSuccess = false,
                    Message = Message.UNKNOW_ERROR_PREFIX + ex.Message,
                };
            }
        }

        public async Task<StatusMessage> GetByConditionsAsync(Predicate<MedicalRecord> condition)
        {
            try
            {
                return new StatusMessage
                {
                    IsSuccess = true,
                    Message = Message.GET_SUCCESS,
                    Data = await _context.MedicalRecords.Where(c => condition(c)).ToListAsync(),
                };
            }
            catch (Exception ex)
            {
                return new StatusMessage
                {
                    IsSuccess = false,
                    Message = Message.UNKNOW_ERROR_PREFIX + ex.Message,
                };
            }
        }

        public async Task<StatusMessage> GetByIdAsync(int id)
        {
            try
            {
                return new StatusMessage
                {
                    IsSuccess = true,
                    Message = Message.GET_SUCCESS,
                    Data = await _context.MedicalRecords
                                .FirstOrDefaultAsync(c => c.MedicalRecordId == id)
                };
            }
            catch (Exception ex)
            {
                return new StatusMessage
                {
                    IsSuccess = false,
                    Message = Message.UNKNOW_ERROR_PREFIX + ex.Message,
                };
            }
        }

        public async Task<StatusMessage> UpdateAsync(MedicalRecord entity)
        {
            try
            {
                var currentEntity = await _context.MedicalRecords.FirstOrDefaultAsync(c => c.MedicalRecordId == entity.MedicalRecordId);

                if (currentEntity == null)
                {
                    return new StatusMessage()
                    {
                        IsSuccess = false,
                        Message = Message.ID_NOT_FOUND
                    };
                }

                //Update properties
                currentEntity.ModifiedOn = DateTime.Now;
                currentEntity.MedicalDetail = entity.MedicalDetail;
                currentEntity.MedicalResult = entity.MedicalResult;
                currentEntity.BookingOrder = entity.BookingOrder;
                currentEntity.PatientId = entity.PatientId;
                currentEntity.ServeUserId = entity.ServeUserId;
                currentEntity.BookingId = entity.BookingId;
                await _context.SaveChangesAsync();

                return new StatusMessage()
                {
                    IsSuccess = true,
                    Message = Message.UPDATE_SUCCESS,
                    Data = currentEntity
                };
            }
            catch (Exception ex)
            {
                return new StatusMessage
                {
                    IsSuccess = false,
                    Message = Message.UNKNOW_ERROR_PREFIX + ex.Message,
                };
            }
        }
    }
}
