using System.Data.SQLite;

namespace Program 
{

    public abstract class Table<T>
    {
        public string name;
        public SQLiteCommand cmd;

        public Table(string name, SQLiteCommand cmd)
        {
            this.name = name;
            this.cmd = cmd;
        }

        public abstract (string cols, string values) ParseInsert(T item);

        public virtual int Insert(T item)
        {
            (string cols, string values) = ParseInsert(item);
            cmd.CommandText = $"INSERT INTO {name}({cols}) VALUES({values});";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "SELECT last_insert_rowid()";
            int rowId = Convert.ToInt32(cmd.ExecuteScalar());
            cmd.CommandText = $"SELECT * FROM {name} WHERE rowid={rowId}";
            return Convert.ToInt32(cmd.ExecuteScalar());
        }


        public abstract T ParseReader(SQLiteDataReader reader);
        public virtual List<T> SelectAll(string condition="")
        {
            cmd.CommandText = $"SELECT * FROM {name} {condition}";
            List<T> users = new List<T>();
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    users.Add(ParseReader(reader));
                }
            }
            return users;
        }
    }

    public class Users : Table<User>
    {
        public Users(string name, SQLiteCommand cmd) : base(name, cmd) {}

        public override (string cols, string values) ParseInsert(User item)
        {
            return (cols: "username, password", values: $"\"{item.username}\", \"{item.password}\"");
        }

        public override User ParseReader(SQLiteDataReader reader)
        {
            return new User((string)reader["username"], (string)reader["password"], reader.GetInt32(reader.GetOrdinal("uid")));
        }

        public User Select(string username)
        {
            cmd.CommandText = $"SELECT * FROM {name} WHERE username='{username}'";
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    return ParseReader(reader);
                }
            }
            return null;
        }
    }

    public class Messages : Table<Message>
    {
        public Messages(string name, SQLiteCommand cmd) : base(name, cmd) {}

        public override (string cols, string values) ParseInsert(Message item)
        {
            return (cols: "body, timestamp, uid", values: $"\"{item.body}\", \"{item.timestamp}\", {item.uid}");
        }

        public override Message ParseReader(SQLiteDataReader reader)
        {
            return new Message(
                reader.GetInt32(reader.GetOrdinal("message_id")),reader.GetInt32(reader.GetOrdinal("uid")), (string)reader["body"], DateTime.Parse((string)reader["timestamp"]));
        }
    }

    public class DB
    {
        private const string connString = "Data Source=Db.sqlite;Version=3";

        private const string userTableSchema = @"CREATE TABLE IF NOT EXISTS [Users] (
            [uid] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
            [username] TEXT NOT NULL UNIQUE,
            [password] TEXT NOT NULL
        )";

        private const string messageTableSchema = @"CREATE TABLE IF NOT EXISTS [Users] (
            [message_id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
            [uid] INTEGER NOT NULL,
            [body] TEXT NOT NULL,
            [timestamp] TEXT NOT NULL,
            FOREIGN KEY (uid) REFERENCES Users(uid)
        )";

        private static SQLiteCommand cmd;
        public Users users;
        public Messages messages;

        public DB()
        {
            try
            {
                SQLiteConnection conn = new SQLiteConnection(connString);
                conn.Open();
                cmd = new SQLiteCommand(conn);
                cmd.CommandText = userTableSchema;
                cmd.ExecuteNonQuery();

                users = new Users("Users", cmd);
                messages = new Messages("Messages", cmd);
            } catch (Exception e)
            {
                Console.WriteLine($"{e.Source}: {e.Message}");
                throw;
            }

        }
    }
}
