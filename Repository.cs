namespace Program {
    public class Repository {
        public static void SaveKey(int uid, string key, bool pub=true) {
            File.WriteAllText($"./known_hosts/{uid}" + (pub ? ".pub" : ""), key);
        }

        public static string LoadKey(int uid, bool pub=true) {
            return File.ReadAllText($"./known_hosts/{uid}" + (pub ? ".pub" : ""));
        }
    }
}
