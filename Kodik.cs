// // 1) Client B generates RSA keys
// Client clientB = new Client(true);
//
// // 2) Client A generates AES key and encrypts it using Client B's public key
// Client clientA = new Client();
// var (encryptedAES, iv) = clientA.EncryptAESKey(clientB.publicKey);
//
// // 3) Client B decrypts AES key
// clientB.ReceiveAESKey(encryptedAES, iv);
//
// // 4) Client B encrypts message using AES and sends it to A
// string message = "Secret message: Hello Client A!";
// byte[] encryptedMessage = clientB.EncryptMessage(message);
//
// // 5) Client A decrypts the message and prints it
// string decrypted = clientA.DecryptMessage(encryptedMessage);
//
// Console.WriteLine("Message from Client B after decryption:");
// Console.WriteLine(decrypted);

namespace Program {
    public static class Program {
        public static void Main()
        {
        }
    }
}
