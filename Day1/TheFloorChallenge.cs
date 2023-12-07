using System;
using System.IO;
using Internal;

class Program
{
    static void Main()
    {
        try
        {
            string filePath = "/Users/alekskonopacki/Desktop/Projects/AdventOfCode/Day1/Floor.txt";

            string bracketsString = File.ReadAllText(filePath);

            char[] bracketsArray = bracketsString.ToCharArray();

            Console.WriteLine("Nawiasy z pliku:");

            foreach (char bracket in bracketsArray)
            {
                Console.Write(bracket + " ");
            }

            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Wystąpił błąd: {ex.Message}");
        }
    }
}
