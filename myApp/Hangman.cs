using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;

namespace myApp
{
    class Hangman
    {
        static void Main(string[] args){
            game_start();
        }
        static void game_start(){
            int life = 10;
            int attempts = 0;
            List<char> entered_letters = new List<char>();
            Stopwatch timer = new Stopwatch();
            string choosed_capital = capital();
            Console.WriteLine("\n" + choosed_capital); 
            string dash_capital = convert_str_to_dash(choosed_capital);
            timer.Start();

            while(life > 0){
                Console.WriteLine("\nYour life: " + life);
                if(life == 1){
                    country_hint(choosed_capital);
                }
                string checked_capital = capital_with_entered_letters(entered_letters, choosed_capital, dash_capital);
                Console.WriteLine("\n" + checked_capital);
                if(check_if_win(choosed_capital, checked_capital)){
                    timer.Stop();
                    player_stats(timer.ElapsedMilliseconds, attempts);
                    break;
                }
                else if(!word_or_letter()){
                    char user_input = user_guess();
                    
                    if(!entered_letters.Contains(user_input)){
                        entered_letters.Add(user_input);
                        attempts++;
                    }else{
                        Console.WriteLine("\nYou already enter " + user_input);
                        continue;
                    }
                    if(!check_guess(choosed_capital, user_input)){
                        life--;
                        Console.WriteLine("\nBuuu, bad guess.");
                        Console.WriteLine("\nYou lose one life.");
                    }
                }else{
                    string word_guess = whole_word_guess();
                    attempts++;
                    if(check_if_win(choosed_capital, word_guess)){
                        timer.Stop();
                        player_stats(timer.ElapsedMilliseconds, attempts);
                        break;
                    }else{
                        life -= 2;
                        Console.WriteLine("\nBuuu, bad guess.");
                        Console.WriteLine("\nYou lose two lifes.");
                        continue;
                    }
                }
            }
            top_players();
            playAgain();
        }


        static string capital(){
            int capital_idx = 1;
            Random rnd = new Random();
            List<string> capitals = new List<string>();

            using (StreamReader sr = new StreamReader("countries_and_capitals.txt")){
                string line;
                while((line = sr.ReadLine()) != null){
                    string[] capital = line.Split('|');
                    capitals.Add(capital[capital_idx]);
                }
            }
            int index = rnd.Next(capitals.Count);
            string random_capital = capitals[index].ToUpper();
            return random_capital;
        }

        static string convert_str_to_dash(string word){
            char[] dash_word = new char[word.Length];

            for (int i = 0; i < word.Length; i++) {
                if(word[i] == ' '){
                    dash_word[i] = ' ';
                }else{
                    dash_word[i] = '_';
                }
            }
            string dash_string = new string(dash_word);
            return dash_string;
        }

        static char user_guess(){
            Console.Write("\nEnter a letter: ");
            char input = Console.ReadKey().KeyChar;
            return Char.ToUpper(input);
        }

        static bool check_guess(string capital, char guess){
            bool valid_guess = false;
            foreach(char c in capital){
                if(c == guess){
                    valid_guess = true;
                }
            }
            return valid_guess;
        }

        static bool check_if_win(string password, string capital_with_guess){
            if(password.Equals(capital_with_guess)){
                Console.WriteLine("\nCONGRATULATIONS, YOU WIN!!!");
                Console.WriteLine("\nThe password was " + password);
                return true;
            }
            return false;
        }

        static string capital_with_entered_letters(List<char> letters, string capital, string dash_capital){
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

        static void country_hint(string capital){
            int capital_idx = 1;
            int country_idx = 0;
            using (StreamReader sr = new StreamReader("countries_and_capitals.txt")){
                string line;
                while((line = sr.ReadLine()) != null){
                    string[] data_split = line.Split('|');
                    if(capital == data_split[capital_idx].ToUpper()){
                        Console.WriteLine("\nHINT!");
                        Console.WriteLine("\nThe capital of " + data_split[country_idx]);
                    }
                }
            }
        }

        static void playAgain(){
            char play_again;
            char play_again_upper;
            do{
                Console.Write("\nDo you want to play again ? [Y/N]: ");
                play_again = Console.ReadKey().KeyChar;
                play_again_upper = Char.ToUpper(play_again);
                if (play_again_upper=='Y'){
                    game_start();
                }
                else if (play_again_upper=='N'){
                    Environment.Exit(0);
                }else{
                    Console.WriteLine("\nPlease enter only y or n.");
                }
            }while(play_again_upper!='Y' && play_again_upper!='N');
        }

        static bool word_or_letter(){
            bool choosed_option = false;
            char word_or_letter;
            char word_or_letter_upper;
            do{
                Console.Write("\nDo you want to guess whole word[W] or one letter[L]: ");
                word_or_letter = Console.ReadKey().KeyChar;
                word_or_letter_upper = Char.ToUpper(word_or_letter);
                if(word_or_letter_upper == 'W'){
                    choosed_option = true;
                }else if (word_or_letter_upper == 'L'){
                    choosed_option = false;
                }else{
                    Console.WriteLine("\nPlease enter only W or L.");
                }
            }while(word_or_letter_upper!='W' && word_or_letter_upper!='L');
            return choosed_option;
        }

        static string whole_word_guess(){
            Console.Write("\nEnter a word: ");
            string input = Console.ReadLine();
            return input.ToUpper();
        }

        static void player_stats(long timer_elapsed_time, int attempts){
            int dividerMillisecToSec = 1000;
            long elapsed_time = timer_elapsed_time/dividerMillisecToSec;
            Console.WriteLine("\nYou guessed after " + attempts + " attempts. It took you " +  elapsed_time + " seconds");
            save_score_to_file(elapsed_time, attempts);
        }

        static void save_score_to_file(long time, int attempts){
            Console.Write("\nEnter your nickname: ");
            string nickname = Console.ReadLine();
            using (TextWriter writer = new StreamWriter("scores.txt", true)){
                writer.Write("\n" + nickname +  "\t|\t" + time + "\t|\t" + attempts);
            }
        }

        static void top_players(){
            List<string> players_score = new List<string>();
            int top_ten_score = 0;

            using (StreamReader sr = new StreamReader("scores.txt")){
                string line;
                while((line = sr.ReadLine()) != null){
                    players_score.Add(line);
                }
            }
            string[] sorted_score = best_player_sort(players_score);

            Console.WriteLine("\nTop 10 players");
            Console.WriteLine("\nNickname\t|\tTime\t|\tAttempts");
            for(int i = 1; i < sorted_score.Length; i++){
                if(top_ten_score < 10){
                    Console.WriteLine(sorted_score[i]);
                    top_ten_score++;
                }
            }
        }

        static string[] best_player_sort(List<string> player_scores){
            string temp;
            string[] sorted_score = new string[player_scores.Count];
            for(var i = 0 ; i < sorted_score.Length; ++i)
                sorted_score[i] = player_scores[i].ToString();

            for(int i=0; i<sorted_score.Length; i++){ 
                for(int j=1; j<sorted_score.Length; j++){
                    int player_attempts = player_guess_attempts(sorted_score[j-1]);
                    int next_player_attempts = player_guess_attempts(sorted_score[j]);
                    if(player_attempts > next_player_attempts){
                        if(player_attempts!=next_player_attempts){ 
                            temp = sorted_score[j-1];  
                            sorted_score[j-1] = sorted_score[j];  
                            sorted_score[j] = temp;
                        }
                    }else if(player_attempts==next_player_attempts){
                        int playerTime = player_guess_time(sorted_score[j-1]);
                        int nextPlayerTime = player_guess_time(sorted_score[j]);
                        if(playerTime > nextPlayerTime){
                            temp = sorted_score[j-1];  
                            sorted_score[j-1] = sorted_score[j];  
                            sorted_score[j] = temp;
                        }
                    }
                }
            }
            return sorted_score;
        }

        static int player_guess_attempts(string player_score){
            int attempts;
            int attempts_idx = 2;
            string[] player_score_split = player_score.Split('|');
            Int32.TryParse(player_score_split[attempts_idx], out attempts);
            return attempts;
        }

        static int player_guess_time(string player_score){
            int time;
            int time_idx = 1;
            string[] player_score_split = player_score.Split('|');
            Int32.TryParse(player_score_split[time_idx], out time);
            return time;
        }
    }
}