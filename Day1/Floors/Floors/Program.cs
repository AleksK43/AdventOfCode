// The newly-improved calibration document consists of lines of text; each line originally contained a specific calibration value that the Elves now need to recover. On each line, the calibration value can be found by combining the first digit and the last digit (in that order) to form a single two-digit number.

// For example:

// 1abc2
// pqr3stu8vwx
// a1b2c3d4e5f
// treb7uchet
// In this example, the calibration values of these four lines are 12, 38, 15, and 77. Adding these together produces 142.

// Consider your entire calibration document. What is the sum of all of the calibration values?


using System.Text.RegularExpressions;

//string CharVector = "two65eightbkgqcsn91qxkfvg"; 
//string newCharVecor = new string (CharVector.Where(char.IsDigit).ToArray());

string filePath = "/Users/alekskonopacki/Desktop/Projects/AdventOfCode/Day1/InputTrebuchet.txt";

string[] charVector = File.ReadAllLines(filePath);


int Result = 0;

foreach (string value in charVector)
{
    string digitChars = ConvertWordsToNumbers(value);

    //Console.WriteLine(digitChars); 
    if (digitChars.Length > 1)
    {
                char firstDigit = digitChars[0];
                char lastDigit = digitChars[digitChars.Length - 1];
                //Console.WriteLine($"{firstDigit}{lastDigit}");
    
                int concatenatedNumber;
                if (int.TryParse($"{firstDigit}{lastDigit}", out concatenatedNumber))
                {
                    Result = Result + concatenatedNumber; 
                }
                else
                {
                    Console.WriteLine("Nie udało się utworzyć liczby całkowitej.");
                }
    }

    else if (digitChars.Length == 1)
    {
        int concatenatedNumber; 
      char digit = digitChars[0]; 
      if ( int.TryParse($"{digit}{digit}", out concatenatedNumber))
      {
        Result = Result + concatenatedNumber; 
      }else {
        Console.WriteLine("Nie udało się sprarsować wartości"); 
      }
    }
    else
    {
        Console.WriteLine("pusty");
    }
} 
    Console.WriteLine(Result); 

static string ConvertWordsToNumbers(string input)
    {
        Dictionary<string, string> wordToNumberMap = new Dictionary<string, string>
        {
            
            { "one", "1" },
            { "two", "2" },
            { "three", "3" },
            { "four" , "4"},
            { "five" , "5"},
            { "six" , "6"},
            { "seven", "7"},
            { "eight" , "8"},
            { "nine" , "9"}
        };

        string pattern = string.Join("|", wordToNumberMap.Keys);
        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

        string result = regex.Replace(input, match => wordToNumberMap[match.Value.ToLower()]);

        result = Regex.Replace(result, @"\D", "");

        return result;
    }
// Zmiana danych imputu na 
// ("one", "o1e"), ("two", "t2o"), ("three", "t3e"),
// 				("four", "f4r"), ("five", "f5e"), ("six", "s6x"),
// 				// ("seven", "s7n"), ("eight", "e8t"), ("nine", "n9n")