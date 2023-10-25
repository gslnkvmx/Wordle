using Microsoft.Data.Sqlite;

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
            Console.WriteLine("Database opened");
            command = new SqliteCommand();
            command.Connection = connection;
            //command.CommandText = "CREATE TABLE IF NOT EXISTS Users(player_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, name TEXT NOT NULL, password TEXT NOT NULL, word_count INTEGER NOT NULL, status_cd TEXT NOT NULL)";
            //command.ExecuteNonQuery();
            //command.CommandText = "CREATE TABLE IF NOT EXISTS Words(word_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, word TEXT NOT NULL)";
            //command.ExecuteNonQuery();
            //command.CommandText = "CREATE TABLE IF NOT EXISTS Progress(player_id INTEGER NOT NULL PRIMARY KEY, word_id INTEGER NOT NULL, word1_id INTEGER NOT NULL, word2_id INTEGER NOT NULL, word3_id INTEGER NOT NULL, word4_id INTEGER NOT NULL, word5_id INTEGER NOT NULL)";
            //command.ExecuteNonQuery();
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
    }
}
