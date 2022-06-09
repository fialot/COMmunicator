using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Security
{
    /// <summary>
    /// Decrypt / Encrypt password
    /// </summary>
    class Password
    {
        string StrPermutation = "lrbsktodbzs";
        byte Permutation1 = 0x20;
        byte Permutation2 = 0x60;
        byte Permutation3 = 0x18;
        byte Permutation4 = 0x52;

        /// <summary>
        /// Constructor with default permutation
        /// </summary>
        public Password()
        {

        }

        /// <summary>
        /// Constructor with user permutation
        /// </summary>
        /// <param name="StrPermutation">String permutation</param>
        /// <param name="Permutation1">Permutation 1</param>
        /// <param name="Permutation2">Permutation 2</param>
        /// <param name="Permutation3">Permutation 3</param>
        /// <param name="Permutation4">Permutation 4</param>
        public Password(string StrPermutation, byte Permutation1, byte Permutation2, byte Permutation3, byte Permutation4)
        {
            SetPermutation(StrPermutation, Permutation1, Permutation2, Permutation3, Permutation4);
        }

        /// <summary>
        /// Set user permutation
        /// </summary>
        /// <param name="StrPermutation">String permutation</param>
        /// <param name="Permutation1">Permutation 1</param>
        /// <param name="Permutation2">Permutation 2</param>
        /// <param name="Permutation3">Permutation 3</param>
        /// <param name="Permutation4">Permutation 4</param>
        public void SetPermutation(string StrPermutation, byte Permutation1, byte Permutation2, byte Permutation3, byte Permutation4)
        {
            this.StrPermutation = StrPermutation;
            this.Permutation1 = Permutation1;
            this.Permutation2 = Permutation2;
            this.Permutation3 = Permutation3;
            this.Permutation4 = Permutation4;
        }

        /// <summary>
        /// Encrypt password
        /// </summary>
        /// <param name="strData">Password</param>
        /// <returns>Encrypted password</returns>
        public string Encrypt(string strData)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(strData)));
        }
        
        /// <summary>
        /// Decrypt password
        /// </summary>
        /// <param name="strData">Encrypted password</param>
        /// <returns>Password</returns>
        public string Decrypt(string strData)
        {
            try
            {
                return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(strData)));
            }
            catch (Exception)
            {
                return strData;
            }
        }

        /// <summary>
        /// Encrypt password
        /// </summary>
        /// <param name="strData">Password</param>
        /// <returns>Encrypted password</returns>
        public byte[] Encrypt(byte[] strData)
        {
            PasswordDeriveBytes passbytes =
            new PasswordDeriveBytes(StrPermutation,
            new byte[] { Permutation1,
                         Permutation2,
                         Permutation3,
                         Permutation4
            });

            MemoryStream memstream = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = passbytes.GetBytes(aes.KeySize / 8);
            aes.IV = passbytes.GetBytes(aes.BlockSize / 8);

            CryptoStream cryptostream = new CryptoStream(memstream,
            aes.CreateEncryptor(), CryptoStreamMode.Write);
            cryptostream.Write(strData, 0, strData.Length);
            cryptostream.Close();
            return memstream.ToArray();
        }

        /// <summary>
        /// Decrypt password
        /// </summary>
        /// <param name="strData">Encrypted password</param>
        /// <returns>Password</returns>
        public byte[] Decrypt(byte[] strData)
        {
            PasswordDeriveBytes passbytes =
            new PasswordDeriveBytes(StrPermutation,
            new byte[] { Permutation1,
                         Permutation2,
                         Permutation3,
                         Permutation4
            });

            MemoryStream memstream = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = passbytes.GetBytes(aes.KeySize / 8);
            aes.IV = passbytes.GetBytes(aes.BlockSize / 8);

            CryptoStream cryptostream = new CryptoStream(memstream,
            aes.CreateDecryptor(), CryptoStreamMode.Write);
            cryptostream.Write(strData, 0, strData.Length);
            cryptostream.Close();
            return memstream.ToArray();
        }
    }
}
