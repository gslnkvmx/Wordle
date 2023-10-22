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
            
            string NL = Environment.NewLine; // shortcut
            string NORMAL = Console.IsOutputRedirected ? "" : "\x1b[39m";
            string GREEN = Console.IsOutputRedirected ? "" : "\x1b[92m";
            string YELLOW = Console.IsOutputRedirected ? "" : "\x1b[93m";
            Data db = new Data();

            string rword = db.Select_word();
            Game my_game = new Game(rword);
            User user = new User();
            //user.Register();
            //db.Register(user.Name, user.Password);
            Console.WriteLine( user.Name);
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
                    bool[] yellow_l = my_game.RightLetters(word);
                    bool[] green_l = my_game.RightPosLetters(word);

                    for (int i = 0; i < word.Length; i++)
                    {
                        if (green_l[i])
                        {
                            Console.Write($"{GREEN}{word[i]}{NORMAL}");
                        }
                        else if (yellow_l[i])
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
