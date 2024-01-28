using MedRoute.Database;
using MedRoute.Models;
using MedRoute.Models.System;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MedRoute.Repository.Implement
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDBContext _context;

        public BookingRepository(AppDBContext context)
        {
            this._context = context;
        }

        public async Task<StatusMessage> CreateAsync(Booking entity)
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

                await _context.AddAsync<Booking>(entity);
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
                var entity = await _context.Bookings.FirstOrDefaultAsync(c => c.BookingId == id);

                if (entity == null)
                {
                    return new StatusMessage()
                    {
                        IsSuccess = false,
                        Message = Message.ID_NOT_FOUND
                    };
                }

                _context.Bookings.Remove(entity);
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
                var books= _context.Bookings.ToList();
                return new StatusMessage
                {
                    IsSuccess = true,
                    Message = Message.GET_SUCCESS,
                    Data = _context.Bookings
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

        public async Task<StatusMessage> GetByConditionsAsync(Predicate<Booking> condition)
        {
            try
            {
                return new StatusMessage
                {
                    IsSuccess = true,
                    Message = Message.GET_SUCCESS,
                    Data = await _context.Bookings.Where(c => condition(c)).ToListAsync(),
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
                    Data = await _context.Bookings
                                .FirstOrDefaultAsync(c => c.BookingId == id)
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

        public async Task<StatusMessage> UpdateAsync(Booking entity)
        {
            try
            {
                var currentEntity = await _context.Bookings.FirstOrDefaultAsync(c => c.BookingId == entity.BookingId);

                if (currentEntity == null)
                {
                    return new StatusMessage()
                    {
                        IsSuccess = false,
                        Message = Message.ID_NOT_FOUND
                    };
                }

                //Update properties
                currentEntity.Order = entity.Order;
                currentEntity.Date = entity.Date;
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
