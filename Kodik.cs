namespace Program {
    public static class Program {
        public static void Main(string[] args)
        {
<<<<<<< HEAD
            DB db = new DB();

            if (args.Length > 2 && args[1] == "r") {
                switch (args[2]) {
                    case "users":
                        db.users.Drop();
                        return;
                    default:
                        break;
                }
            }

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
        }
    }
}
