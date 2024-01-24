using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace MedRoute.Services
{
    public interface IHashPassword
    {
        public string GetHashPassword(string password);
    }
    public class HashPassword : IHashPassword
    {
        private readonly string SALT = "ThisIsASalt";

        public string GetHashPassword(string password)
        {
            byte[] salt = Encoding.Default.GetBytes(SALT);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
