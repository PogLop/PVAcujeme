// // 1) Client B generates RSA keys
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
        public static void Main(string[] args)
        {
            /*DB db = new DB();

            if (args.Length > 2 && args[1] == "r") {
                switch (args[2]) {
                    case "users":
                        db.users.Drop();
                        return;
                    default:
                        break;
                }
            }

            User admin = db.users.Login("admin", "admin");
            Client client = new Client(admin);
            byte[] publicKey = Convert.FromBase64String(Repository.LoadKey(admin.uid));*/
            
            //  testing: tui.Choose
            /*string[] opt = ["bruh", "heehee", "heehee2"];
            string? x = Tui.Choose("titul", ref opt);
            Console.WriteLine($"x: {x}");*/

            /*Tui.FormField[] form = new Tui.FormField[4] { new Tui.FormField("string", Tui.FormFieldType.String), new Tui.FormField("int", Tui.FormFieldType.Int), new Tui.FormField("double", Tui.FormFieldType.Double), new Tui.FormField("bool", Tui.FormFieldType.Boolean) };

            var ret = Tui.Form("Form", ref form);
            Console.WriteLine($"returned {ret}");*/
        }
    }
}
