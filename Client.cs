using System.Security.Cryptography;
using System.Text;


namespace Program {
    class Client
    {
        // RSA
        public byte[] publicKey;
        private byte[] privateKey;

        // AES
        private byte[] aesKey;
        private byte[] aesIV;

        const int KEY_LENGTH = 2048;
        RSAEncryptionPadding PADDING = RSAEncryptionPadding.Pkcs1;

        public Client(int uid, bool gen)
        {
            if (gen) {
                using (var rsa = RSA.Create(KEY_LENGTH))
                {
                    publicKey = rsa.ExportRSAPublicKey();
                    privateKey = rsa.ExportRSAPrivateKey();
                }
                Repository.SaveKey(uid, Convert.ToBase64String(publicKey));
                Repository.SaveKey(uid, Convert.ToBase64String(privateKey), false);
                return;
            }
            using (var rsa = RSA.Create(KEY_LENGTH))
            {
                this.publicKey = Convert.FromBase64String(Repository.LoadKey(uid));
                this.privateKey = Convert.FromBase64String(Repository.LoadKey(uid, false));
            }
        }

        public void ReceiveAESKey(byte[] encryptedKey, byte[] iv)
        {
            using (var rsa = RSA.Create(KEY_LENGTH))
            {
                rsa.ImportRSAPrivateKey(privateKey, out _);
                aesKey = rsa.Decrypt(encryptedKey, PADDING); 
            }

            aesIV = iv;
        }

        // Client A: encrypt AES key for Client B
        public (byte[] encryptedKey, byte[] iv) EncryptAESKey(byte[] publicKeyB)
        {
            using (Aes aes = Aes.Create())
            {
                aes.GenerateKey();
                aes.GenerateIV();
                aesKey = aes.Key;
                aesIV = aes.IV;

                using (var rsa = RSA.Create(KEY_LENGTH)) {
                    rsa.ImportRSAPublicKey(publicKeyB, out _);
                    byte[] encryptedKey = rsa.Encrypt(aesKey, PADDING);
                    return (encryptedKey, aesIV);
                }
            }
        }
        // Encrypt message using AES
        public byte[] EncryptMessage(string message)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = aesKey;
                aes.IV = aesIV;
                ICryptoTransform encryptor = aes.CreateEncryptor();
                byte[] plainBytes = Encoding.UTF8.GetBytes(message);
                return encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            }
        }

        // Decrypt message using AES
        public string DecryptMessage(byte[] encryptedMessage)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = aesKey;
                aes.IV = aesIV;
                ICryptoTransform decryptor = aes.CreateDecryptor();
                byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedMessage, 0, encryptedMessage.Length);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }
}
