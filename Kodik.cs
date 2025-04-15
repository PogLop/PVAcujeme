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

        public Client(bool genKey=false)
        {
            if (genKey) {
                using (var rsa = RSA.Create(KEY_LENGTH))
                {
                    publicKey = rsa.ExportRSAPublicKey();
                    privateKey = rsa.ExportRSAPrivateKey();
                }
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

    public static class Program {
        public static void Main()
        {
            // 1) Client B generates RSA keys
            Client clientB = new Client(true);

            // 2) Client A generates AES key and encrypts it using Client B's public key
            Client clientA = new Client();
            var (encryptedAES, iv) = clientA.EncryptAESKey(clientB.publicKey);

            // 3) Client B decrypts AES key
            clientB.ReceiveAESKey(encryptedAES, iv);

            // 4) Client B encrypts message using AES and sends it to A
            string message = "Secret message: Hello Client A!";
            byte[] encryptedMessage = clientB.EncryptMessage(message);

            // 5) Client A decrypts the message and prints it
            string decrypted = clientA.DecryptMessage(encryptedMessage);

            Console.WriteLine("Message from Client B after decryption:");
            Console.WriteLine(decrypted);
        }
    }
}
