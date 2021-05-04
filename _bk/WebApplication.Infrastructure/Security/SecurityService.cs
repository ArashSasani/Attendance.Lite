using WebApplication.Infrastructure.Interfaces;

namespace WebApplication.Infrastructure.Security
{
    public class SecurityService : ISecurityService
    {
        private HashComputer m_hashComputer = new HashComputer();
        public string GeneratePasswordHash(string plainTextPassword, out string salt)
        {
            salt = SaltGenerator.GetSaltString();
            string finalString = plainTextPassword + salt;
            return m_hashComputer.GetCrypt(finalString);
        }

        public string GeneratePasswordHash(string plainTextPassword, byte[] keyBytes)
        {
            string finalString = plainTextPassword + m_hashComputer.GetStringKey(keyBytes);
            return m_hashComputer.GetCrypt(finalString);
        }

        public bool IsPasswordMatch(string password, string salt, string hash)
        {
            string finalString = password + salt;
            return m_hashComputer.IsMatch(finalString, hash);
        }

        public bool IsPasswordMatch(string password, byte[] keyBytes, string hash)
        {
            string finalString = password + m_hashComputer.GetStringKey(keyBytes);
            return m_hashComputer.IsMatch(finalString, hash);
        }
    }
}
