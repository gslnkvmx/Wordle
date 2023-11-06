namespace wordle
{
    internal class User
    {
        int player_id;
        string? name;
        string? password;
        int word_count = 0;
        string status_cd = "N";

        public User(int player_id, string name, string password, int word_count, string status_cd)
        {
            this.player_id = player_id;
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.password = password ?? throw new ArgumentNullException(nameof(password));
            this.word_count = word_count;
            this.status_cd = status_cd ?? throw new ArgumentNullException(nameof(status_cd));
        }

        public User()
        {
        }

        public User(string name, int word_count)
        {
            this.name = name;
            this.word_count = word_count;
        }

        public User(User user_data)
        {
            player_id = user_data.player_id;
            name = user_data.name ?? throw new ArgumentNullException(nameof(user_data));
            password = user_data.password ?? throw new ArgumentNullException(nameof(user_data));
            word_count = user_data.word_count;
            status_cd = user_data.status_cd ?? throw new ArgumentNullException(nameof(user_data));

        }

        public int Player_id
        {
            get { return player_id; }
            set { player_id = value; }
        }

        public string Name
        {
            get { return name!; }
            set { name = value; }
        }

        public string Password
        {
            get { return password!; }
            set { password = value; }
        }
        public int Word_count
        {
            get { return word_count; }
            set { word_count = value; }
        }
        public string Status_cd
        {
            get { return status_cd; }
            set { status_cd = value; }
        }

        public void Set_Name()
        {
            Console.WriteLine("Введите имя: ");
            bool check;
            do
            {
                Name = Console.ReadLine();
                check = String.IsNullOrWhiteSpace(Name);
                if (check) Console.WriteLine("Имя не должно быть пустым");


            }
            while (check);
        }

        public void Set_Password()
        {
            bool check;
            Console.WriteLine("Введите пароль: ");
            do
            {
                Password = Console.ReadLine();
                check = String.IsNullOrWhiteSpace(Password);
                if (check) Console.WriteLine("Пароль не должен быть пустым");
            }
            while (check);
        }

        public void Register()
        {
            Console.WriteLine("Введите имя: ");
            bool check;
            do
            {
                name = Console.ReadLine();
                check = String.IsNullOrWhiteSpace(name);
                if (check) Console.WriteLine("Имя не должно быть пустым");
            }
            while (check);
            Console.WriteLine("Введите пароль: ");
            do
            {
                password = Console.ReadLine();
                check = String.IsNullOrWhiteSpace(password);
                if (check) Console.WriteLine("Пароль не должен быть пустым");
            }
            while (check);
        }

        public override string ToString()
        {
            return $"Имя: {Name}, угаданных слов: {Word_count}";
        }

    }
}
