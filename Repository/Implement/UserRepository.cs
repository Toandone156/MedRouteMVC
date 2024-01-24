using MedRoute.Database;
using MedRoute.Models;
using MedRoute.Models.System;
using MedRoute.Services;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MedRoute.Repository.Implement
{
    public class UserRepository : IUserRepository, IAuthenticateService
    {
        private readonly AppDBContext _context;
        private readonly IHashPassword _hash;
        public UserRepository(AppDBContext context, IHashPassword hash)
        {
            this._context = context;
            this._hash = hash;
        }

        public async Task<StatusMessage> CheckEmailAndUsernameAsync(string email, string? username)
        {
            if (_context.Users.Any(x => (x.Email == email)
                                            || ((username != null)
                                                && (x.UserName == username))))
            {
                return new StatusMessage()
                {
                    IsSuccess = true,
                    Message = "Success",
                    Data = true
                };
            }

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = "Success",
                Data = false
            };
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

        public async Task<StatusMessage> RegisterAsync(Register register)
        {
            if (register.Password != register.AgainPassword)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = "Password was not match"
                };
            }

            if (_context.Users.Any(x => x.Email == register.Email))
            {
                return new StatusMessage
                {
                    IsSuccess = false,
                    Message = "Email was existed",
                    Data = null
                };
            }

            var user = new User()
            {
                UserName = register.Username,
                HashPassword = _hash.GetHashPassword(register.Password),
                FullName = register.FullName,
                Email = register.Email,
                RoleId = register.RoleId ?? 1,
                UserId = 0 //New user
            };

            await _context.AddAsync<User>(user);
            await _context.SaveChangesAsync();
            return new StatusMessage()
            {
                IsSuccess = true,
                Message = "Register success",
                Data = user
            };
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

        public async Task<StatusMessage> ValidateLoginAsync(Login login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => (u.UserName == login.Username) || (u.Email == login.Username));
            if (user == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = "Username, Email or Password was wrong!"
                };
            }

            if (user.HashPassword != _hash.GetHashPassword(login.Password))
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = "Username, Email or Password was wrong!"
                };
            }

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = "Login success!",
                Data = user
            };
        }
    }
}
