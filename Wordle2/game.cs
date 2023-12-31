﻿namespace wordle
{
    internal class Game
    {
        string rword;
        int move = 0;

        public Game(string rword)
        {
            this.rword = rword ?? throw new ArgumentNullException(nameof(rword));
            move = 1;
        }

        public Game(string rword, int move)
        {
            this.rword = rword ?? throw new ArgumentNullException(nameof(rword));
            this.move = move;
        }

        public Game() { rword = "None"; }
        public string Right_word { get { return rword; } set { rword = value; } }
        public int Move { get { return move; } set { move = value; } }

        public bool CheckRword(string word)
        {
            if (!rword.Equals(word))
            {
                move += 1;
            }
            return rword.Equals(word);
        }
        /// <summary>
        /// Метод проверяет должна ли данная буква быть выделена желтым цветом
        /// </summary>
        /// <param name="index">индекс данной буквы в передаваемом слове</param>
        /// <param name="word"></param>
        /// <returns>True если буква должна быть выделена желтым</returns>
        bool CheckForYellow(ref int index, string word)
        {
            int letterCount = 0;
            int incorrectCountBeforeIndex = 0;
            int correctCount = 0;
            for (int i = 0; i < rword.Length; i++)
            {
                if (rword[i] == word[index])
                {
                    letterCount++;
                }
                if (word[i] == word[index] && rword[i] == word[index])
                {
                    correctCount++;
                }
                if (i < index && word[i] == word[index] && rword[i] != word[index])
                {
                    incorrectCountBeforeIndex++;
                }
            }
            index++;
            return letterCount - correctCount - incorrectCountBeforeIndex > 0;
        }

        public bool[] RightLetters(string word)
        {
            bool[] letters_id = new bool[word.Length];
            for (int i = 0; i < word.Length;)
            {
                letters_id[i] = CheckForYellow(ref i, word);
            }
            return letters_id;
        }

        public bool[] RightPosLetters(string word)
        {
            bool[] letters_id = new bool[word.Length];
            for (int i = 0; i < word.Length; i++)
            {
                letters_id[i] = (rword[i] == word[i]);
            }
            return letters_id;
        }

    }
}
