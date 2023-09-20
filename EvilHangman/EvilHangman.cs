#nullable disable

public class EvilHangman
{

    // Normal hangman
    public static void Hangman()
    {
        // Define variables
        string path = @"C:\Users\David Wang\Desktop\Projects\App Development\Unit1\Unit1\resources\dictionary.txt";
        List<string> dictionaryWords = new List<string>();

        // Read dictionary words
        using (StreamReader sr = File.OpenText(path))
        {
            string s = "";
            while((s = sr.ReadLine()) != null)
            {
                dictionaryWords.Add(s);
            }
        }

        // Prompt user and choose 



    }

}