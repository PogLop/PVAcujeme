namespace Program
{
    public class User
    {
        public int uid;
        public string username;
        public string password;

        public User(string username, string password, int uid=0)
        {
            this.username = username;
            this.password = password;
            this.uid = uid;
        }
    }

    public class Message
    {
        public int message_id;
        public int uid;
        public string body;
        public DateTime timestamp;

        public Message(int message_id, int uid, string body, DateTime timestamp)
        {
            this.message_id = message_id;
            this.uid = uid;
            this.body = body;
            this.timestamp = timestamp;
        }
    }
}
