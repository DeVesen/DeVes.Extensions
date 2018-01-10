using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DeVes.Extensions.Helpers
{
    public static class Cryptography
    {
        public static string CreateHash(string unHashed)
        {
            var _x = new MD5CryptoServiceProvider();

            var _data = Encoding.ASCII.GetBytes(unHashed);
            _data = _x.ComputeHash(_data);

            return Encoding.ASCII.GetString(_data);
        }
        public static bool MatchHash(string hashData, string hashUser)
        {
            hashUser = CreateHash(hashUser);
            return hashUser == hashData;
        }


        public static string Encrypt(string clearText, string encryptionKey)
        {
            var _clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (var _encryptor = Aes.Create())
            {
                if (_encryptor == null) return clearText;

                var _pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

                _encryptor.Key = _pdb.GetBytes(32);
                _encryptor.IV = _pdb.GetBytes(16);
                using (var _ms = new MemoryStream())
                {
                    using (var _cs = new CryptoStream(_ms, _encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        _cs.Write(_clearBytes, 0, _clearBytes.Length);
                        _cs.Close();
                    }
                    clearText = Convert.ToBase64String(_ms.ToArray());
                }
            }
            return clearText;
        }
        public static string Decrypt(string cipherText, string encryptionKey)
        {
            var _cipherBytes = Convert.FromBase64String(cipherText);
            using (var _encryptor = Aes.Create())
            {
                if (_encryptor == null) return cipherText;

                var _pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

                _encryptor.Key = _pdb.GetBytes(32);
                _encryptor.IV = _pdb.GetBytes(16);
                using (var _ms = new MemoryStream())
                {
                    using (var _cs = new CryptoStream(_ms, _encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        _cs.Write(_cipherBytes, 0, _cipherBytes.Length);
                        _cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(_ms.ToArray());
                }
            }
            return cipherText;
        }

        public static string EncryptObj(object objectToEncrypt, string encryptionKey)
        {
            var _jsonFormat = JsonConvert.SerializeObject(objectToEncrypt);
            return Encrypt(_jsonFormat, encryptionKey);
        }
        public static T DecryptObj<T>(string cipherText, string encryptionKey)
        {
            var _clearText = Decrypt(cipherText, encryptionKey);
            return JsonConvert.DeserializeObject<T>(_clearText);
        }
        public static JObject DecryptObj(string cipherText, string encryptionKey)
        {
            var _clearText = Decrypt(cipherText, encryptionKey);
            return JsonConvert.DeserializeObject(_clearText) as JObject;
        }
    }
}
