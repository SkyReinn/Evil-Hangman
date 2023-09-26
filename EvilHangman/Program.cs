using System;
using System.Globalization;

class Program
{
    public static void Main(string[] args)
    {
        EvilHangman.InitializeWords("dictionary.txt");
        EvilHangman.Hangman();
    }
}