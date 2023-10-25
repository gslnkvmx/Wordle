using static System.Runtime.InteropServices.JavaScript.JSType;

namespace wordle
{
    internal class Program
    {
        public static void ClearCurrentConsoleLine(int row)
        {
            //currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, row);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, row);
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

            int top = Console.CursorTop;
            int y = top;

            Console.WriteLine("Зарегистрироваться");
            Console.WriteLine("Войти");
            Console.WriteLine("Выйти из игры");

            int down = Console.CursorTop;

            Console.CursorSize = 100;
            Console.CursorTop = top;

            ConsoleKey key;
            while ((key = Console.ReadKey(true).Key) != ConsoleKey.Enter)
            {
                if (key == ConsoleKey.UpArrow)
                {
                    if (y > top)
                    {
                        y--;
                        Console.CursorTop = y;
                    }
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    if (y < down - 1)
                    {
                        y++;
                        Console.CursorTop = y;
                    }
                }
            }

            Console.CursorTop = down;

            if (y == top)
            {
                user.Register();
                db.Register(user.Name, user.Password);
            }
            else if (y == top + 1)
                Console.WriteLine("два");
            else if (y == top + 2)
                Console.WriteLine("три");

            Console.WriteLine(user.Name);
            Console.WriteLine("Отгадайте слово");
            Console.WriteLine($"Загаданное слово: {rword}");
            while (true & my_game.GetMove() <= 6)
            {
                string word;
                bool check = false;
                int crow = Console.CursorTop;
                do
                {
                    word = Console.ReadLine();
                    check = db.Check_word(word);
                    if (check) Console.WriteLine("Такого слова нет, попробуйте другое!");
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    ClearCurrentConsoleLine(crow);
                    ClearCurrentConsoleLine(Console.CursorTop);
                }
                while (check);

                Console.SetCursorPosition(0, Console.CursorTop - 1);
                ClearCurrentConsoleLine(crow);
                ClearCurrentConsoleLine(Console.CursorTop+1);
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                if (my_game.CheckWord(word))
                {
                    Console.WriteLine($"{GREEN}{word}{NORMAL}, правильно!");
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
            if (my_game.GetMove() > 6) Console.WriteLine($"Загаданное слово: {rword}");
        }
    }
}
