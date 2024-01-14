using MedRoute.Database;
using MedRoute.Models;
using MedRoute.Models.System;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MedRoute.Repository.Implement
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDBContext _context;
        public RoleRepository(AppDBContext context)
        {
            this._context = context;
        }

        public async Task<StatusMessage> CreateAsync(Role entity)
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

                await _context.AddAsync<Role>(entity);
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
                var entity = await _context.Roles.FirstOrDefaultAsync(c => c.RoleId == id);

                if (entity == null)
                {
                    return new StatusMessage()
                    {
                        IsSuccess = false,
                        Message = Message.ID_NOT_FOUND
                    };
                }

                _context.Roles.Remove(entity);
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
                    Data = _context.Roles
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

        public async Task<StatusMessage> GetByConditionsAsync(Predicate<Role> condition)
        {
            try
            {
                return new StatusMessage
                {
                    IsSuccess = true,
                    Message = Message.GET_SUCCESS,
                    Data = await _context.Roles.Where(c => condition(c)).ToListAsync(),
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
                    Data = await _context.Roles
                                .FirstOrDefaultAsync(c => c.RoleId == id)
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

        public async Task<StatusMessage> UpdateAsync(Role entity)
        {
            try
            {
                var currentEntity = await _context.Roles.FirstOrDefaultAsync(c => c.RoleId == entity.RoleId);

                if (currentEntity == null)
                {
                    return new StatusMessage()
                    {
                        IsSuccess = false,
                        Message = Message.ID_NOT_FOUND
                    };
                }

                //Update properties
                currentEntity.RoleName = entity.RoleName;
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
