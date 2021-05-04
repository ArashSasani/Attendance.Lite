using System;

namespace WebApplication.Infrastructure
{
    public static class Utility
    {
        public static string GetString(byte[] data)
        {
            return Convert.ToBase64String(data, 0, data.Length);
        }

        public static byte[] GetBytes(string value)
        {
            int length = value.Length * sizeof(char);
            byte[] bytes = null;
            if (length < AppSettings.BLOWFISH_KEY_BYTES_SIZE)
                bytes = new byte[length];
            else
                bytes = new byte[AppSettings.BLOWFISH_KEY_BYTES_SIZE];

            Buffer.BlockCopy(value.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
