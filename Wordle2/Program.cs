namespace wordle
{
    internal class Program
    {
        static readonly string NORMAL = Console.IsOutputRedirected ? "" : "\x1b[39m";
        static readonly string GREEN = Console.IsOutputRedirected ? "" : "\x1b[92m";
        static readonly string YELLOW = Console.IsOutputRedirected ? "" : "\x1b[93m";

        public static void ClearCurrentConsoleLine(int row)
        {
            Console.SetCursorPosition(0, row);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, row);
        }

        public static int Menu()
        {
            Console.WriteLine("------Меню------");
            Console.WriteLine("1. Зарегистрироваться");
            Console.WriteLine("2. Войти");
            Console.WriteLine("0. Выход");
            Console.WriteLine("------------------------");
            return GetValue(2);
        }

        public static int GameMenu()
        {
            Console.WriteLine("------Меню------");
            Console.WriteLine("1. Начать игру");
            Console.WriteLine("2. Таблица лидеров");
            Console.WriteLine("0. Выход");
            Console.WriteLine("------------------------");
            return GetValue(2);
        }

        public static int GameMenuContinue()
        {
            Console.WriteLine("------Меню------");
            Console.WriteLine("1. Продолжить игру");
            Console.WriteLine("2. Начать игру");
            Console.WriteLine("3. Таблица лидеров");
            Console.WriteLine("0. Выход");
            Console.WriteLine("------------------------");
            return GetValue(3);
        }
        private static void ColorWord(Game my_game, string word)
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

        public static int GetValue(int max)
        {
            Console.WriteLine("Чтобы выбрать, введите число:");
            var input = Console.ReadLine();
            bool success = Int32.TryParse(input, out int result);
            if (success & result <= max & 0 <= result)
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

        static void Main()
        {
            Data db = new();

            User user = new();

            bool logined = false;
            bool registrated;
            int choice = Menu();

            switch (choice)
            {
                case (int)Options.Register: goto Register;
                case (int)Options.Login: goto Login;
                case (int)Options.Exit: goto Exit;
            }

        Register:
            do
            {
                Console.Clear();
                user.SetName();
                registrated = db.CheckName(user.Name);
                if (registrated)
                {
                    Console.WriteLine("Такой пользователь уже есть!");
                    Console.WriteLine("1. Ввести другое имя\n2. Войти\n0. Выход");
                    switch (GetValue(2))
                    {
                        case 1:
                            registrated = false;
                            break;
                        case 2:
                            goto Login;
                        case 0:
                            goto Exit;
                    }

                }
                else
                {
                    user.SetPassword();
                    registrated = db.Register(user.Name, user.Password);
                    if (!registrated)
                    {
                        Console.WriteLine("1. Заново\n2. Войти\n0. Выход из игры");
                        switch (GetValue(2))
                        {
                            case 1:
                                break;
                            case 2:
                                goto Login;
                            case 0:
                                goto Exit;
                        }
                    }
                }
            } while (!registrated);
            logined = true;
            goto Login;

        Login:
            Console.Clear();
            if (logined) Console.WriteLine("Вход выполнен!");
            else
            {
                do
                {
                    Console.Clear();
                    user.SetName();
                    logined = db.CheckName(user.Name);
                    if (!logined)
                    {
                        Console.WriteLine("Такого пользователя нет!");
                        Console.WriteLine("1. Ввести другое имя\n2. Регистрация\n0. Выход");
                        switch (GetValue(2))
                        {
                            case 1:
                                break;
                            case 2:
                                goto Register;
                            case 0:
                                goto Exit;
                        }

                    }
                    else
                    {
                        user.SetPassword();
                        logined = db.CheckPassword(user.Name, user.Password);
                        if (!logined)
                        {
                            Console.WriteLine("Неверный пароль, повторите попытку!");
                            Console.WriteLine("1. Войти заново\n2. Регистрация\n0. Выход");
                            switch (GetValue(2))
                            {
                                case 1:
                                    break;
                                case 2:
                                    goto Register;
                                case 0:
                                    goto Exit;
                            }
                        }
                    }
                } while (!logined);
            }
            goto Menu;

        Exit:
            System.Environment.Exit(1);

        Menu:
            Console.Clear();
            user = db.UserData(user.Name);
            //Console.WriteLine(user);
            if (user.Status_cd == "N")
            {
                switch (GameMenu())
                {
                    case 1: goto Game;
                    case 2: goto Leaderboard;
                    case 0: goto Exit;
                }
            }

            else if (user.Status_cd == "UACT")
            {
                switch (GameMenuContinue())
                {
                    case 1: goto Game;
                    case 2:
                        user.Status_cd = "N";
                        db.ClearProgress(user.Name);
                        goto Game;
                    case 3: goto Leaderboard;
                    case 0: goto Exit;
                }
            }

        Leaderboard:
            Console.Clear();
            bool intop = false;
            List<User> rate_list = db.Rating();
            for (int i = 0; i < rate_list.Count; i++)
            {
                if (rate_list[i].Name == user.Name)
                {
                    intop = true;
                    Console.WriteLine($"{GREEN}Ты -> {user}{NORMAL}");
                }
                else Console.WriteLine(rate_list[i]);
            }

            if (!intop) Console.WriteLine($"=====================================\n" +
                $"{GREEN}Ты -> {user}{NORMAL}");
            Console.WriteLine("1. Назад\n0. Выход");
            switch (GetValue(1))
            {
                case 1:
                    goto Menu;
                case 0:
                    goto Exit;
            }

        Game:
            Game my_game;
            Console.Clear();
            Console.WriteLine("Угадайте загаданное слово из ПЯТИ букв с шести попыток!\n" +
                "После каждой попытки цвет букв будет менятся, чтобы показать какие буквы есть в загаданном слове.\n" +
                "Буквы выделенные " + $"{YELLOW}желтым {NORMAL}" + "есть в заданном слове, но стоят не на том месте.\n" +
                "Буквы выделенные " + $"{GREEN}зеленым {NORMAL}" + "есть в заданном слове, и стоят в правильном месте.\n" +
                "\nЧтобы выйти введите 0!");
            //Console.WriteLine($"Загаданное слово: {rword}");

            if (user.Status_cd == "UACT")
            {
                var progress = db.GetProgress(user.Name);
                my_game = new Game(progress[0], progress.Count);
                for (int i = 1; i < progress.Count; i++)
                {
                    ColorWord(my_game, progress[i]);
                }
            }
            else
            {
                string rword = db.SelectWord();
                my_game = new Game(rword);
                user.Status_cd = "UACT";
                db.AddRword(rword, user.Name);
            }

            while (true & my_game.Move <= 6)
            {
                string? word;
                int crow = Console.CursorTop;
                bool check;
                db.SetUserData(user);
                do
                {
                    word = Console.ReadLine();
                    if (word == "0") break;
                    db.CheckWord(word, out check);
                    if (check) Console.WriteLine("Такого слова нет, попробуйте другое!");
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    ClearCurrentConsoleLine(crow);
                    ClearCurrentConsoleLine(Console.CursorTop);
                }
                while (check);

                if (word == "0") { Console.Clear(); break; }

                if (my_game.Move != 6) db.AddProgress(my_game.Move, word!, user.Name);

                Console.SetCursorPosition(0, Console.CursorTop - 1);
                ClearCurrentConsoleLine(crow);
                ClearCurrentConsoleLine(Console.CursorTop + 1);
                Console.SetCursorPosition(0, Console.CursorTop - 1);

                if (my_game.CheckRword(word!))
                {
                    Console.WriteLine($"{GREEN}{word}{NORMAL}, правильно!");
                    user.Word_count += 1;
                    db.ClearProgress(user.Name);
                    user.Status_cd = "N";
                    break;
                }

                else
                    ColorWord(my_game, word!);

            }
            if (my_game.Move > 6)
            {
                Console.WriteLine($"Загаданное слово: {my_game.Right_word}");
                db.ClearProgress(user.Name);
                user.Status_cd = "N";
            }
            db.SetUserData(user);
            Console.WriteLine("1. В меню\n0. Выход");
            switch (GetValue(1))
            {
                case 1:
                    goto Menu;
                case 0:
                    goto Exit;
            }
        }
    }
}
