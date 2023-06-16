using System.Security.Cryptography;
using System.Text;

namespace RPG.BuildingBlocks.Common.Utils
{
    public static class CryptoUtils
    {
        public static string GetSha256Hash(string text)
        {
            var result = "";

            using (SHA256 mySHA256 = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] hashValue = mySHA256.ComputeHash(enc.GetBytes(text));

                result = ByteToStringConvert(hashValue);
            }

            return result;
        }

        private static string ByteToStringConvert(byte[] bytes)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append($"{bytes[i]:X2}".ToLower());
            }
            return sb.ToString();
        }
    }
}