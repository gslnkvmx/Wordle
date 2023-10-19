using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wordle
{
    internal class Program
    {
        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
        static void Main(string[] args)
        {
            using (var connection = new SqliteConnection("Data Source=database.db"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                command.CommandText = "CREATE TABLE IF NOT EXISTS Users(player_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, name TEXT NOT NULL, password TEXT NOT NULL, word_count INTEGER NOT NULL, status_cd TEXT NOT NULL)";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE IF NOT EXISTS Words(word_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, word TEXT NOT NULL)";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE IF NOT EXISTS Progress(player_id INTEGER NOT NULL PRIMARY KEY, word_id INTEGER NOT NULL, word1_id INTEGER NOT NULL, word2_id INTEGER NOT NULL, word3_id INTEGER NOT NULL, word4_id INTEGER NOT NULL, word5_id INTEGER NOT NULL)";
                command.ExecuteNonQuery();
            }
            string NL = Environment.NewLine; // shortcut
            string NORMAL = Console.IsOutputRedirected ? "" : "\x1b[39m";
            string GREEN = Console.IsOutputRedirected ? "" : "\x1b[92m";
            string YELLOW = Console.IsOutputRedirected ? "" : "\x1b[93m";

            Game my_game = new Game("apple");
            Console.WriteLine("Отгадайте слово");
            while (true & my_game.GetMove()<=6)
            {
                string word = Console.ReadLine();
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                ClearCurrentConsoleLine();
                if (my_game.CheckWord(word))
                {
                    Console.WriteLine($"{GREEN}{word}{NORMAL}");
                    break;
                }

                else
                {
                    int[] yellow_l = my_game.RightLetters(word);
                    int[] green_l = my_game.RightPosLetters(word);

                    for (int i = 0; i < word.Length; i++)
                    {
                        if (green_l[i] == 1)
                        {
                            Console.Write($"{GREEN}{word[i]}{NORMAL}");
                        }
                        else if (yellow_l[i] == 1)
                        {
                            Console.Write($"{YELLOW}{word[i]}{NORMAL}");
                        }
                        else
                        {
                            Console.Write(word[i]);
                        }
                    }
                    Console.WriteLine();
                }
            }

        }
    }
}
