using MedRoute.Database;
using MedRoute.Models;
using MedRoute.Models.System;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MedRoute.Repository.Implement
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDBContext _context;
        public UserRepository(AppDBContext context)
        {
            this._context = context;
        }

        public async Task<StatusMessage> CreateAsync(User entity)
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

                await _context.AddAsync<User>(entity);
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
                var entity = await _context.Users.FirstOrDefaultAsync(c => c.UserId == id);

                if (entity == null)
                {
                    return new StatusMessage()
                    {
                        IsSuccess = false,
                        Message = Message.ID_NOT_FOUND
                    };
                }

                _context.Users.Remove(entity);
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
                    Data = _context.Users
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

        public async Task<StatusMessage> GetByConditionsAsync(Predicate<User> condition)
        {
            try
            {
                return new StatusMessage
                {
                    IsSuccess = true,
                    Message = Message.GET_SUCCESS,
                    Data = await _context.Users.Where(c => condition(c)).ToListAsync(),
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
                    Data = await _context.Users
                                .FirstOrDefaultAsync(c => c.UserId == id)
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

        public async Task<StatusMessage> UpdateAsync(User entity)
        {
            try
            {
                var currentEntity = await _context.Users.FirstOrDefaultAsync(c => c.UserId == entity.UserId);

                if (currentEntity == null)
                {
                    return new StatusMessage()
                    {
                        IsSuccess = false,
                        Message = Message.ID_NOT_FOUND
                    };
                }

                //Update properties
                currentEntity.RoleId = entity.RoleId;
                currentEntity.UserName = entity.UserName;
                currentEntity.HashPassword = entity.HashPassword;
                currentEntity.FullName = entity.FullName;
                currentEntity.CitizenIdentificationCardNumber = entity.CitizenIdentificationCardNumber;
                currentEntity.Email = entity.Email;
                currentEntity.DateOfBirth = entity.DateOfBirth;
                currentEntity.Gender = entity.Gender;
                currentEntity.Job = entity.Job;
                currentEntity.PhoneNumber = entity.PhoneNumber;
                currentEntity.Nation = entity.Nation;
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
