using MedRoute.Models;
using MedRoute.Models.System;

namespace MedRoute.Repository
{
    public interface IAuthenticateService
    {
        public Task<StatusMessage> ValidateLoginAsync(Login login);
        public Task<StatusMessage> RegisterAsync(Register register);
        public Task<StatusMessage> CheckEmailAndUsernameAsync(string email, string? username);
    }
}
