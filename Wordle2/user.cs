using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wordle
{
    internal class User
    {
        int player_id;
        string name;
        string password;
        int word_count;
        string status_cd;

        public User(int player_id, string name, string password, int word_count, string status_cd)
        {
            this.player_id = player_id;
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.password = password ?? throw new ArgumentNullException(nameof(password));
            this.word_count = word_count;
            this.status_cd = status_cd ?? throw new ArgumentNullException(nameof(status_cd));
        }

        public int Player_id
        {
            get { return player_id; }
            set { player_id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Password
        {
            get { return password; }
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
    }
}
