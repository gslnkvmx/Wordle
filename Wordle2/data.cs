using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Windows.Input;
using System.Xml.Linq;

namespace wordle
{
    internal class Data
    {
        SqliteConnection connection;
        SqliteCommand command;
        public Data()
        {
            connection = new SqliteConnection("Data Source=database.db");
            connection.Open();
            command = new SqliteCommand();
            command.Connection = connection;
            //command.CommandText = "CREATE TABLE IF NOT EXISTS Users(player_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, name TEXT NOT NULL, password TEXT NOT NULL, word_count INTEGER NOT NULL, status_cd TEXT NOT NULL)";
            //command.ExecuteNonQuery();
            //command.CommandText = "CREATE TABLE IF NOT EXISTS Words(word_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, word TEXT NOT NULL)";
            //command.ExecuteNonQuery();
            //command.CommandText = "CREATE TABLE IF NOT EXISTS Progress(player_id INTEGER NOT NULL PRIMARY KEY, word_id INTEGER NOT NULL, word1_id INTEGER NOT NULL, word2_id INTEGER NOT NULL, word3_id INTEGER NOT NULL, word4_id INTEGER NOT NULL, word5_id INTEGER NOT NULL)";
            //command.ExecuteNonQuery();
        }

        public User User_data(string name)
        {
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

            return new User(user_id,
                            name: user_name,
                            password: user_password,
                            word_count: user_word_count,
                            status_cd: user_status);
        }

        public bool Check_name(string name)
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

            return String.IsNullOrEmpty(out_name);
        }

        public bool Register(string name, string password)
        {
            try
            {
                command.CommandText = "INSERT INTO Users (name, password, status_cd) VALUES (:name, :password, :status)";
                command.Parameters.AddWithValue("name", name);
                command.Parameters.AddWithValue("password", password);
                command.Parameters.AddWithValue("status", "N");
                command.ExecuteNonQuery();
                Console.WriteLine("User added");
                return true;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine("User not added");
                return false;
            }
        }

        public bool Check_password(string name, string password)
        {
            command.CommandText = string.Format("SELECT name FROM Users WHERE name = '{0}' and password = '{1}'", name, password);
            string? out_name = null;
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    out_name = reader.GetString(0);
                }
            }

            return String.IsNullOrEmpty(out_name);
        }

        public string Select_word()
        {
            Random rnd = new Random();

            //Получить случайное число (в диапазоне от 1 до 182)
            int id = rnd.Next(1, 182);

            command.CommandText =
     @"
        SELECT word
        FROM Words
        WHERE word_id = $id
    ";
            command.Parameters.AddWithValue("$id", id);
            string word = "not found";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    word = reader.GetString(0);
                }
            }
            return word;
        }

        public bool Check_word(string word)
        {
            command.CommandText = string.Format("SELECT word FROM Words WHERE word = '{0}'", word);
            string? out_word = null;
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    out_word = reader.GetString(0);
                }
            }
            return String.IsNullOrEmpty(out_word);
        }

        public List<User> Rating(int top)
        {
            var rate_list = new List<User>();
            command.CommandText = string.Format("SELECT name, word_count FROM Users ORDER BY word_count DESC LIMIT {0}", top);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    rate_list.Add(new User(reader.GetString(0), reader.GetInt32(1)));
                }
            }
            return rate_list;
        }
    }
}
