namespace WebApplication.Infrastructure.Interfaces
{
    public interface ISecurityService
    {
        string GeneratePasswordHash(string plainTextPassword, out string salt);
        string GeneratePasswordHash(string plainTextPassword, byte[] keyBytes);
        bool IsPasswordMatch(string password, string salt, string hash);
        bool IsPasswordMatch(string password, byte[] keyBytes, string hash);
    }
}
