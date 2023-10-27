using System;
using System.Collections.Generic;

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

        public static int Menu()
        {
            Console.WriteLine("------Меню------");
            Console.WriteLine("1. Зарегестрироваться");
            Console.WriteLine("2. Войти");
            Console.WriteLine("0. Выход");
            Console.WriteLine("------------------------");
            return GetValue(2);
        }

        public static int Game_Menu()
        {
            Console.WriteLine("------Меню------");
            Console.WriteLine("1. Начать игру");
            Console.WriteLine("2. Таблица лидеров");
            Console.WriteLine("0. Выход");
            Console.WriteLine("------------------------");
            return GetValue(2);
        }

        public static int Game_Menu_continue()
        {
            Console.WriteLine("------Меню------");
            Console.WriteLine("1. Продолжить игру");
            Console.WriteLine("2. Начать игру");
            Console.WriteLine("3. Таблица лидеров");
            Console.WriteLine("0. Выход");
            Console.WriteLine("------------------------");
            return GetValue(3);
        }

        public static int GetValue(int max)
        {
            Console.WriteLine("Чтобы выбрать, введите число:");
            string input = Console.ReadLine();
            bool success = Int32.TryParse(input, out int result);
            if (success & result<=max & 0 <= result)
            {
                return result;
            }
            else
            {
                Console.WriteLine("Некорректное значение");
                return GetValue(max);
            }
        }

        public enum Options
        {
            Exit,
            Register,
            Login
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

            const int top = 0;
            int y = top;
            bool registrated = false;
            bool logined = false;
            int choise = Menu();

            switch (choise)
            {
                case (int)Options.Register: goto Register;
                case (int)Options.Login: goto Login;
                case (int)Options.Exit: goto Exit;
            }
            
        Register:
            do
            {
                Console.Clear();
                user.Set_Name();
                registrated = db.Check_name(user.Name);
                if (!registrated)
                {
                    Console.WriteLine("Такой пользователь уже есть!");
                    Console.WriteLine("1. Ввести другое имя    2. Войти    0. Выход");
                    switch (GetValue(2))
                    {
                        case 1:
                            break;
                        case 2:
                            goto Login;
                        case 0:
                            System.Environment.Exit(1);
                            break;
                    }

                }
                else
                {
                    user.Set_Password();
                    registrated = db.Register(user.Name, user.Password);
                    logined = true;
                }
            } while (!registrated);
            goto Login;

        Login:
            Console.Clear();
            if (registrated) Console.WriteLine("Вход выполнен!");
            else
            {
                do
                {
                    Console.Clear();
                    user.Set_Name();
                    logined = db.Check_name(user.Name);
                    if (logined)
                    {
                        Console.WriteLine("Такого пользователя нет!");
                        Console.WriteLine("1. Ввести другое имя    2. Регистрация    0. Выход");
                        switch (GetValue(2))
                        {
                            case 1:
                                break;
                            case 2:
                                goto Register;
                            case 0:
                                System.Environment.Exit(1);
                                break;
                        }

                    }
                    else
                    {
                        user.Set_Password();
                        logined = db.Check_password(user.Name, user.Password);
                        if (logined)
                        {
                            Console.WriteLine("Неверный пароль, повторите попытку!");
                            Console.WriteLine("1. Войти заново    2. Регистрация    0. Выход");
                            switch (GetValue(2))
                            {
                                case 1:
                                    break;
                                case 2:
                                    goto Register;
                                case 0:
                                    System.Environment.Exit(1);
                                    break;
                            }
                        }
                    }
                } while (logined);
            }
            goto Game;

        Exit:
            System.Environment.Exit(1);

        Game:
            int ingame_choice = Game_Menu();
            int num_top = 5;
            List<User> rate_list = db.Rating(num_top);
            rate_list.ForEach(Console.WriteLine);

            user = db.User_data(user.Name); 
            Console.WriteLine("Отгадайте слово");
            //Console.WriteLine($"Загаданное слово: {rword}");
            while (true & my_game.Move <= 6)
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
                ClearCurrentConsoleLine(Console.CursorTop + 1);
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
            if (my_game.Move > 6) Console.WriteLine($"Загаданное слово: {my_game.Right_word}");
        }
    }
}
