using System.Security.Cryptography;
using System.Text;

namespace TimeTable.DataContext.Models
{
    public class PasswordManager
    {
        public static string HashPassword(string password, string salt)
        {
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(salt)))
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashedBytes = hmac.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public static string GenerateSalt()
        {
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzAnhHieuDepTrai442350123456789";
        }
    }
}
