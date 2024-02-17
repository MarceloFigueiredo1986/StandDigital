using System.Security.Cryptography;
using System.Text;

namespace StandDigital.Encryption;

public class DataEncryption
{
    #region Hashing

    public string Hash(string valueToHash)
    {
        var combinedPassword = string.Concat(valueToHash, GetSalt());
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(combinedPassword);
            byte[] hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
    private string GetSalt() => "88UM4hDqMwsw4fE6SNtKwQ==";

    #endregion

    #region Encryption

    public static string Encrypt(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Convert.FromBase64String("ZsHWyzuzafG67LIImTum1NvczAFHZHfSPDiXa+HV1UQ=");
            aes.GenerateIV();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Write(aes.IV, 0, aes.IV.Length);

                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                {
                    streamWriter.Write(plainText);
                }

                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }

    public static string Decrypt(string cipherText)
    {
        byte[] cipherBytes = Convert.FromBase64String(cipherText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Convert.FromBase64String("ZsHWyzuzafG67LIImTum1NvczAFHZHfSPDiXa+HV1UQ=");

            using (MemoryStream memoryStream = new MemoryStream(cipherBytes))
            {
                byte[] iv = new byte[aes.IV.Length];
                memoryStream.Read(iv, 0, iv.Length);
                aes.IV = iv;

                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (StreamReader streamReader = new StreamReader(cryptoStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }

    #endregion
}
