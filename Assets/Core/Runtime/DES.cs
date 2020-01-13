
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace MC.Core
{
    internal class DES
    {
        public DES(byte[] _keyVector)//密钥向量,8位就好了例如new byte[]{0x01,0x02,0x03,0x04,0x05,0x05,0x07}
        {
            keyVector = _keyVector;
        }

        private readonly byte[] keyVector;

        public string DesEncrypt(string normalTxt, string EncryptKey)
        {
            var bytes = Encoding.Default.GetBytes(normalTxt);
            var key = Encoding.UTF8.GetBytes(EncryptKey.PadLeft(8, '0').Substring(0, 8));
            using (MemoryStream ms = new MemoryStream())
            {
                var encry = new DESCryptoServiceProvider();
                CryptoStream cs = new CryptoStream(ms, encry.CreateEncryptor(key, keyVector), CryptoStreamMode.Write);
                cs.Write(bytes, 0, bytes.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public string DesDecrypt(string securityTxt, string EncryptKey)//解密
        {
            try
            {
                var bytes = Convert.FromBase64String(securityTxt);
                var key = Encoding.UTF8.GetBytes(EncryptKey.PadLeft(8, '0').Substring(0, 8));
                using (MemoryStream ms = new MemoryStream())
                {
                    var descrypt = new DESCryptoServiceProvider();
                    CryptoStream cs = new CryptoStream(ms, descrypt.CreateDecryptor(key, keyVector), CryptoStreamMode.Write);
                    cs.Write(bytes, 0, bytes.Length);
                    cs.FlushFinalBlock();
                    return Encoding.UTF8.GetString(ms.ToArray());
                }

            }
            catch (Exception exception)
            {
                Debug.LogError($"MSG: {exception.Message} Source:{exception.Source}");
                return string.Empty;
            }
        }
    }
}