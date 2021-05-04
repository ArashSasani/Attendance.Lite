using System.Security.Cryptography;

namespace WebApplication.Infrastructure
{
    public static class SaltGenerator
    {
        private static RNGCryptoServiceProvider m_cryptoServiceProvider = null;

        static SaltGenerator()
        {
            m_cryptoServiceProvider = new RNGCryptoServiceProvider();
        }

        public static string GetSaltString()
        {
            byte[] saltBytes = new byte[AppSettings.PASS_SALT_SIZE];
            m_cryptoServiceProvider.GetNonZeroBytes(saltBytes);
            string saltString = Utility.GetString(saltBytes);
            return saltString;
        }
    }
}
