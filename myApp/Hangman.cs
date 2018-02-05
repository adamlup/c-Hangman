using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace myApp
{
    class Hangman
    {
        static void Main(string[] args)
        {
            int life = 10;
            List<char> entered_letters = new List<char>();
            string choosed_capital = capital();
            Console.WriteLine(choosed_capital);
            string dash_capital = convert_str_to_dash(choosed_capital);
            Console.WriteLine(dash_capital);
            while(life > 0){
                char user_input = user_guess();
                Console.WriteLine("\n" + user_input);
                entered_letters.Add(user_input);
                string checked_capital = capital_with_entered_letters(entered_letters, choosed_capital, dash_capital);
                Console.WriteLine(checked_capital);
            }
            Console.ReadKey();

        }

        static string capital(){
            Random rnd = new Random();
            List<string> capitals = new List<string>();

            using (StreamReader sr = new StreamReader("countries_and_capitals.txt")){
                string line;
                while((line = sr.ReadLine()) != null){
                    string[] capital = line.Split('|');
                    capitals.Add(capital[1]);
                }
            }
            int index = rnd.Next(capitals.Count);
            string random_capital = capitals[index].ToUpper();
            return random_capital;
        }

        static string convert_str_to_dash(string word){
            char[] dash_word = new char[word.Length];

            for (int i = 0; i < word.Length; i++) {
                dash_word[i] = '_';
            }
            string dash_string = new string(dash_word);
            return dash_string;
        }

        static char user_guess(){
            Console.Write("Enter a letter: ");
            char input = Console.ReadKey().KeyChar;
            return Char.ToUpper(input);
        }

        static String capital_with_entered_letters(List<char> letters, string capital, string dash_capital){
            StringBuilder sb = new StringBuilder(dash_capital);
            for(int i = 0; i<letters.Count; i++){
                for(int j = 0; j<capital.Length; j++){
                    if(letters[i]==capital[j]){
                        sb[j] = letters[i];
                    }
                }
            }
            string capital_with_guess_letters = sb.ToString();
            return capital_with_guess_letters;
        }
    }
}
