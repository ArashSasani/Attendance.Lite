using CryptSharp;

namespace WebApplication.Infrastructure
{
    public class HashComputer
    {
        private byte[] GetKeyBytes(string token)
        {
            return Utility.GetBytes(token);
        }

        public string GetStringKey(byte[] data)
        {
            return Utility.GetString(data);
        }

        public string GetCrypt(string message)
        {
            return Crypter.Blowfish.Crypt(GetKeyBytes(message));

            #region Simple SHA1
            //SHA256 sha = new SHA256CryptoServiceProvider();
            //byte[] dataBytes = Utility.GetBytes(message);
            //byte[] resultBytes = sha.ComputeHash(dataBytes);
            //return Utility.GetString(resultBytes);
            #endregion
        }

        public bool IsMatch(string message, string crypted)
        {
            return (crypted == Crypter.Blowfish.Crypt(GetKeyBytes(message), crypted));
        }
    }
}
