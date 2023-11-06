using Microsoft.Data.Sqlite;

namespace wordle
{
    internal class Data
    {
        readonly SqliteConnection connection;
        readonly SqliteCommand command;
        const int NUM_WORDS = 4165;
        public Data()
        {
            connection = new SqliteConnection("Data Source=database.db");
            command = new SqliteCommand
            {
                Connection = connection
            };
            //command.CommandText = "CREATE TABLE IF NOT EXISTS Users(player_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, name TEXT NOT NULL, password TEXT NOT NULL, word_count INTEGER NOT NULL, status_cd TEXT NOT NULL)";
            //command.ExecuteNonQuery();
            //command.CommandText = "CREATE TABLE IF NOT EXISTS Words(word_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, word TEXT NOT NULL)";
            //command.ExecuteNonQuery();
            //command.CommandText = "CREATE TABLE IF NOT EXISTS Progress(player_id INTEGER NOT NULL PRIMARY KEY, word_id INTEGER NOT NULL, word1_id INTEGER NOT NULL, word2_id INTEGER NOT NULL, word3_id INTEGER NOT NULL, word4_id INTEGER NOT NULL, word5_id INTEGER NOT NULL)";
            //command.ExecuteNonQuery();
        }

        public User User_data(string name)
        {
            connection.Open();
            command.CommandText = string.Format("SELECT * FROM Users WHERE name = '{0}'", name);
            int user_id = 0;
            string? user_name = null;
            string? user_password = null;
            int user_word_count = 0;
            string user_status = "N";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    user_id = reader.GetInt32(0);
                    user_name = reader.GetString(1);
                    user_password = reader.GetString(2);
                    user_word_count = reader.GetInt32(3);
                    user_status = reader.GetString(4);
                }
            }
            connection.Close();

            return new User(user_id,
                            name: user_name!,
                            password: user_password!,
                            word_count: user_word_count!,
                            status_cd: user_status!);
        }

        public void Set_user_data(User user)
        {
            connection.Open();
            command.CommandText = string.Format("UPDATE Users SET word_count = {0}, status_cd = '{1}' WHERE name = '{2}'",
                user.Word_count, user.Status_cd, user.Name);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public bool Check_name(string name)
        {
            connection.Open();
            try
            {
                command.CommandText = string.Format("SELECT name FROM Users WHERE name = '{0}'", name);
                string? out_name = null;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        out_name = reader.GetString(0);
                    }
                }
                connection.Close();
                return !String.IsNullOrEmpty(out_name); //возвращает true если пользователь с именем name уже существует, иначе - false
            }
            catch (SqliteException)
            {
                connection.Close();
                return false;
            }
        }

        public bool Register(string name, string password)
        {
            connection.Open();
            try
            {
                command.CommandText = string.Format("INSERT INTO Users (name, password) VALUES ('{0}', '{1}')", name, password);
                command.ExecuteNonQuery();
                command.CommandText = string.Format("INSERT INTO Progress (name) VALUES ('{0}')", name);
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (SqliteException)
            {
                Console.WriteLine("Недопустимое имя или пароль!");
                connection.Close();
                return false;
            }
        }

        public bool Check_password(string name, string password)
        {
            connection.Open();
            command.CommandText = string.Format("SELECT name FROM Users WHERE name = '{0}' and password = '{1}'", name, password);
            string? out_name = null;
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    out_name = reader.GetString(0);
                }
            }
            connection.Close();
            return !String.IsNullOrEmpty(out_name);
        }

        public string Select_word()
        {
            connection.Open();
            Random rnd = new();

            //Получить случайное число (в диапазоне от 1 до 40000)
            int id = rnd.Next(1, NUM_WORDS);

            command.CommandText = string.Format(" SELECT word FROM Words WHERE word_id = {0}", id);
            string word = "not found";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    word = reader.GetString(0);
                }
            }
            connection.Close();
            return word;
        }

        public bool Check_word(string? word)
        {
            connection.Open();
            command.CommandText = string.Format("SELECT word FROM Words WHERE word = '{0}'", word);
            string? out_word = null;
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    out_word = reader.GetString(0);
                }
            }
            connection.Close();
            return String.IsNullOrEmpty(out_word);
        }

        public List<User> Rating(int top = 5)
        {
            connection.Open();
            var rate_list = new List<User>();
            command.CommandText = string.Format("SELECT name, word_count FROM Users ORDER BY word_count DESC LIMIT {0}", top);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    rate_list.Add(new User(reader.GetString(0), reader.GetInt32(1)));
                }
            }
            connection.Close();
            return rate_list;
        }

        public void Add_progress(int move, string word, string name)
        {
            connection.Open();
            command.CommandText = string.Format("UPDATE Progress SET word{0} = '{1}' WHERE name = '{2}'", move, word, name);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void Add_rword(string rword, string name)
        {
            connection.Open();
            command.CommandText = string.Format("UPDATE Progress SET rword = '{0}' WHERE name = '{1}'", rword, name);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public List<string> Get_progress(string name)
        {
            connection.Open();
            var progress = new List<string>();
            int i = 1;
            command.CommandText = string.Format("SELECT * FROM Progress WHERE name = '{0}'", name);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    while (i <= 6)
                    {
                        if (reader[i].GetType() != typeof(DBNull)) { progress.Add(reader.GetString(i)); }
                        else break;
                        i += 1;
                    }

                }
            }
            connection.Close();
            return progress;
        }

        public void Clear_progress(string name)
        {
            connection.Open();
            command.CommandText = string.Format("DELETE FROM Progress WHERE name = '{0}'", name);
            command.ExecuteNonQuery();
            command.CommandText = string.Format("INSERT INTO Progress (name) VALUES ('{0}')", name);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
