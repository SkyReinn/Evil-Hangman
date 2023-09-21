#nullable disable

public class EvilHangman
{

    // Normal hangman
    public static void Hangman()
    {
        // Define variables
        string path = Directory.GetCurrentDirectory() + @"\EvilHangman\dictionary.txt";
        List<string> dictionaryWords = new List<string>();
        List<string> words = new List<string>();
        List<char> guessedWord = new List<char>();
        var wordLength = 0;
        var guesses = 7;
        string answer;
        bool win = false;

        // Read dictionary words
        using (StreamReader sr = File.OpenText(path))
        {
            string s = "";
            while((s = sr.ReadLine()) != null)
            {
                dictionaryWords.Add(s.ToLower());
            }
        }

        // Prompt player to enter word length
        Console.WriteLine("Welcome to the game of hangman!\nPlease enter your word length:");
        wordLength = int.Parse(Console.ReadLine());

        // Select words from dictionary words
        foreach(var word in dictionaryWords)
        {
            if(word.Length == wordLength)
            {
                words.Add(word);
            }
        }

        // Select a random word from words (temporary)
        var random = new Random();
        answer = words[random.Next(words.Count)];

        // Initialize guessed word
        for(var i=0;i<wordLength;i++)
        {
            guessedWord.Add('_');
        }

        // Guessing
        while(guesses != 0)
        {
            // Prompt player to guess a letter
            Console.WriteLine("You have {0} chances left, please guess a letter:", guesses);
            char guessedLetter = char.Parse(Console.ReadLine());

            // Iterate through the answer to check if any letter are correct
            for(var i=0;i<wordLength;i++)
            {
                if(guessedLetter == answer[i])
                {
                    guessedWord[i] = guessedLetter;
                }
            }

            // Check if the answer is guessed correctly
            bool flag = true;
            for(var i=0;i<wordLength;i++)
            {
                if(guessedWord[i] == '_')
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

            // Display guessed word
            foreach(var letter in guessedWord)
            {
                Console.Write(letter);
            }
            Console.Write('\n');

            guesses--;
        }

        // Print results
        if(win == true)
            Console.WriteLine("Congratulations! You have correctly guessed the word.");
        else
            Console.WriteLine("You lost, the answer is {0}.", answer);
    }
}