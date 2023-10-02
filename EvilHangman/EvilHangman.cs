#nullable disable 

using System.Linq.Expressions;
using System.Text;


public class EvilHangman
{
    // Define global variables 
    public static string path = Directory.GetCurrentDirectory() + @"\EvilHangman\";
    public static List<string> dictionaryWords = new List<string>();
    public static List<string> words = new List<string>();
    public static int guesses = 6;
    public static int wordLength;


    // Why the heck is string read-only in c# 
    public static string ReplaceAtIndex(string str, int index, char c)
    {
        StringBuilder sb = new StringBuilder(str);
        sb[index] = c;
        return sb.ToString();
    }


    // Initialize a word bank containing all words of a certain length 
    public static void InitializeWords(string file)
    {

        // Read dictionary words 
        using (StreamReader sr = File.OpenText(path + file))
        {
            string s = "";
            while ((s = sr.ReadLine()) != null)
            {
                dictionaryWords.Add(s.ToLower());
            }
        }

        // Prompt player to enter word length 
        Console.WriteLine("Welcome to the game of Hangman!\nPlease enter your word length:");
        wordLength = int.Parse(Console.ReadLine());

        // Select words from dictionary words 
        foreach (var word in dictionaryWords)
        {
            if (word.Length == wordLength)
            {
                words.Add(word);
            }
        }
    }


    // Main game function 
    public static void Hangman()
    {

        // Define variables 
        List<char> guessedWord = new List<char>();
        string answer = "";
        bool win = false;
        StringBuilder hangman = new StringBuilder();

        // Initialize variables
        for (var i = 0; i < wordLength; i++)
            guessedWord.Add('_');
        for (var i = 0; i < wordLength; i++)
            answer += '_';

        // Game loop
        while (guesses != 0)
        {
            char guessedLetter = '\0';
            bool validInput = false;
            List<char> guessedLetters = new List<char>();

            while (!validInput)
            { 
                // Draw the hangman
                updateHangman(hangman, guesses);

                // Prompt user to guess a letter
                Console.WriteLine("You have {0} guess{1} left, please guess a letter: ", guesses, guesses != 1 ? "es" : null);
                string input = Console.ReadLine();
                
                // Check if the input is valid
                if (input.Length == 1 && char.IsLetter(input[0]))
                {
                    guessedLetter = char.Parse(input);
                    if (!guessedLetters.Contains(guessedLetter))
                    {
                        guessedLetters.Add(guessedLetter);
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("You've already guessed {0}. Guess another letter", guessedLetter);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a single letter.");
                }
            }

            // Iterate through the answer to check if any letters are correct
            for (var i = 0; i < wordLength; i++)
                if (guessedLetter == answer[i])
                    guessedWord[i] = guessedLetter;

            // Find the answer pattern with the most words
            answer = EvilAlgorithm(guessedLetter, new string(guessedWord.ToArray()));

            // Check if the answer is guessed correctly
            bool flag = true;
            for(var i = 0; i < wordLength; i++)
            {
                if (guessedWord[i] == '_')
                {
                    flag = false;
                    break;
                }
            }
            if(flag == true)
            {
                win = true;
                break;
            }

            // Display the guessed word
            foreach(var letter in guessedWord)
            {
                Console.Write(letter);
            }
            Console.Write('\n');

            guesses--;
        }

        // Display results
        if (win == true)
            Console.WriteLine("Congratulations! You have correctly guessed the word.");
        else
        {
            // Choose a random word as the answer from the remaining words
            var random = new Random();
            var ans = random.Next(words.Count);
            updateHangman(hangman, 0);
            Console.Write("You lost, the answer is {0}", words[ans]);
        }
    }


    // Draw the hangman
    public static void updateHangman(StringBuilder hangman, int incorrectGuesses)
    {
        hangman.Clear();
        switch (incorrectGuesses)
        {
            case 0:
                hangman.AppendLine("_______");
                hangman.AppendLine("|");
                hangman.AppendLine("|");
                hangman.AppendLine("|");
                hangman.AppendLine("|");
                hangman.AppendLine("|");
                hangman.AppendLine("|");
                hangman.AppendLine("_______");
                break;
            case 1:
                hangman.AppendLine("_______");
                hangman.AppendLine("|  |");
                hangman.AppendLine("|  |");
                hangman.AppendLine("|  o");
                hangman.AppendLine("|");
                hangman.AppendLine("|");
                hangman.AppendLine("|");
                hangman.AppendLine("_______");
                break;
            case 2:
                hangman.AppendLine("_______");
                hangman.AppendLine("|  |");
                hangman.AppendLine("|  |");
                hangman.AppendLine("|  o");
                hangman.AppendLine("|  |");
                hangman.AppendLine("|");
                hangman.AppendLine("|");
                hangman.AppendLine("_______");
                break;

            case 3:
                hangman.AppendLine("_______");
                hangman.AppendLine("|  |");
                hangman.AppendLine("|  |");
                hangman.AppendLine("|  o");
                hangman.AppendLine("| /|");
                hangman.AppendLine("|");
                hangman.AppendLine("|");
                hangman.AppendLine("_______");
                break;

            case 4:
                hangman.AppendLine("_______");
                hangman.AppendLine("|  |");
                hangman.AppendLine("|  |");
                hangman.AppendLine("|  o");
                hangman.AppendLine(@"| /|\");
                hangman.AppendLine("|");
                hangman.AppendLine("|");
                hangman.AppendLine("_______");
                break;
            case 5:
                hangman.AppendLine("_______");
                hangman.AppendLine("|  |");
                hangman.AppendLine("|  |");
                hangman.AppendLine("|  o");
                hangman.AppendLine(@"| /|\");
                hangman.AppendLine("| /");
                hangman.AppendLine("_______");
                break;
            case 6:
                hangman.AppendLine("_______");
                hangman.AppendLine("|  |");
                hangman.AppendLine("|  |");
                hangman.AppendLine("|  o");
                hangman.AppendLine(@"| /|\");
                hangman.AppendLine(@"| / \");
                hangman.AppendLine("_______");
                break;
            default:
                break;
        }
        Console.WriteLine(hangman);
    }


    // Evil Algorithm
    public static string EvilAlgorithm(char guessedLetter, string guessedWord)
    {
        // Define the dictionary
        Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();

        // Loop through each word 
        foreach (var word in words)
        {
            // Check if the word pattern matches letters already guessed 
            bool flag = true;
            for (var i = 0; i < wordLength; i++)
                if (guessedWord[i] != '_')
                    if (guessedWord[i] != word[i])
                        flag = false;

            // If the pattern matches 
            if (flag == true)
            {
                // Create a new pattern based on the word 
                string pattern = guessedWord;
                for (var i = 0; i < wordLength; i++)
                    if (word[i] == guessedLetter)
                        pattern = ReplaceAtIndex(pattern, i, guessedLetter);

                // If the pattern already exists, add the word 
                if (dictionary.Keys.Contains(pattern))
                {
                    List<string> temp = dictionary[pattern];
                    temp.Add(word);
                    dictionary[pattern] = temp;
                }

                // If not, create a new key then add the word 
                else
                {
                    List<string> temp = new List<string> { word };
                    dictionary.Add(pattern, temp);
                }
            }
        }

        // Find the pattern in the dictionary that contains the most words 
        var max = 0;
        string answer = guessedWord;
        foreach (var key in dictionary.Keys)
        {
            // Debugging 
            // Console.WriteLine("There are {0} words that fit the pattern '{1}'", dictionary[key].Count, key);

            if (dictionary[key].Count > max)
            {
                max = dictionary[key].Count;
                answer = key;
            }
        }

        // Delete all the words that match the other patterns from total words 
        foreach (var key in dictionary.Keys)
            if (key != answer)
                foreach (var word in dictionary[key])
                    words.Remove(word);

        return answer;
    }
}