using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wordle
{
    internal class Game
    {
        string rword;
        int move;

        public Game(string rword)
        {
            this.rword = rword ?? throw new ArgumentNullException(nameof(rword));
            move = 1;
        }

        public bool CheckWord(string word)
        {
            if (!rword.Equals(word))
            {
                move += 1;
            }
            return rword.Equals(word);
        }

        public int[] RightLetters(string word)
        {
            int[] letters_id = new int[word.Length];
            for (int i = 0; i < word.Length; i++)
            {
                if (rword.Contains(word[i]))
                {
                    letters_id[i] = 1;
                }
            }
            return letters_id;
        }

        public int[] RightPosLetters(string word)
        {
            int[] letters_id = new int[word.Length];
            for (int i = 0; i < word.Length; i++)
            {
                if (rword[i] == word[i])
                {
                    letters_id[i] = 1;
                }
            }
            return letters_id;
        }


        public int GetMove() { return move; }

    }
}
