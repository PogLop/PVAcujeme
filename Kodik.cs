namespace Program {
    public static class Program {
        public static void Main(string[] args)
        {
<<<<<<< HEAD
<<<<<<< HEAD
            DB db = new DB();
=======
            /*DB db = new DB();
>>>>>>> origin/matysek

            if (args.Length > 2 && args[1] == "r") {
                switch (args[2]) {
                    case "users":
                        db.users.Drop();
                        return;
                    default:
                        break;
                }
            }

<<<<<<< HEAD
            User u = db.users.Signin("user", "user");
            User c = db.users.Signin("admin", "admin");
            Client user;
            Client admin;

            if (u == null || c == null) {
                u = db.users.Login("user", "user");
                c = db.users.Login("admin", "admin");
                user = new Client(u.uid, false);
                admin = new Client(c.uid, false);
            } else {
                user = new Client(u.uid, false);
                admin = new Client(c.uid, false);
            }

            var (key, iv) = admin.EncryptAESKey(Convert.FromBase64String(Repository.LoadKey(u.uid)));
            user.ReceiveAESKey(key, iv);
            byte[] encryptedMessage = admin.EncryptMessage("Hello World!");
            string decryptedMessage = user.DecryptMessage(encryptedMessage);
            Console.WriteLine(decryptedMessage);
=======
>>>>>>> origin/matysek
=======
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
>>>>>>> origin/matysek
        }
    }
}
