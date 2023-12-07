using System;
using System.Collections.Generic;
using System.IO;

class EngineSchematicAnalyzer
{
    private readonly (int, int)[] DIRECTIONS =
    {
        (-1, -1), // Top-left
        (-1, 0),  // Up
        (-1, 1),  // Top-right
        (0, -1),  // Left
        (0, 1),   // Right
        (1, -1),  // Bottom-left
        (1, 0),   // Down
        (1, 1)    // Bottom-right
    };

    private readonly char[][] schematic;
    private readonly HashSet<(int, int)> processedLocations = new HashSet<(int, int)>();

    public EngineSchematicAnalyzer(string schematic)
    {
        var lines = schematic.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        this.schematic = new char[lines.Length][];

        for (int i = 0; i < lines.Length; i++)
        {
            this.schematic[i] = lines[i].ToCharArray();
        }
    }

    public long CalculateSumOfPartNumbers()
    {
        long totalSum = 0;

        for (int i = 0; i < schematic.Length; i++)
        {
            int j = 0;
            while (j < schematic[i].Length)
            {
                if (char.IsDigit(schematic[i][j]) && !processedLocations.Contains((i, j)))
                {
                    int partNumber = GetFullPartNumber(i, j);
                    processedLocations.UnionWith(
                        Enumerable.Range(j, partNumber.ToString().Length).Select(colIndex => (i, colIndex))
                    );

                    if (Enumerable.Range(j, partNumber.ToString().Length)
                        .Any(colIndex => IsAdjacentToSymbol(i, colIndex)))
                    {
                        totalSum += partNumber;
                    }

                    j += partNumber.ToString().Length - 1;
                }

                j++;
            }
        }

        return totalSum;
    }

    public long CalculateSumOfAllGearRatios()
    {
        long totalSum = 0;

        for (int i = 0; i < schematic.Length; i++)
        {
            for (int j = 0; j < schematic[i].Length; j++)
            {
                totalSum += schematic[i][j] == '*' ? GetGearRatio(i, j) : 0;
            }
        }

        return totalSum;
    }

    private long GetGearRatio(int row, int col)
    {
        var adjacentNumbers = new List<int>();

        foreach (var (dx, dy) in DIRECTIONS)
        {
            int adjacentRow = row + dx;
            int adjacentCol = col + dy;

            if (0 <= adjacentRow && adjacentRow < schematic.Length &&
                0 <= adjacentCol && adjacentCol < schematic[adjacentRow].Length &&
                char.IsDigit(schematic[adjacentRow][adjacentCol]))
            {
                int partNumber = GetFullPartNumber(adjacentRow, adjacentCol);

                if (!adjacentNumbers.Contains(partNumber))
                {
                    adjacentNumbers.Add(partNumber);
                }
            }
        }

        return adjacentNumbers.Count == 2 ? (long)adjacentNumbers[0] * adjacentNumbers[1] : 0;
    }

    private int GetFullPartNumber(int row, int col)
    {
        string numberStr = schematic[row][col].ToString();

        int leftCol = col - 1;
        while (leftCol >= 0 && char.IsDigit(schematic[row][leftCol]))
        {
            numberStr = schematic[row][leftCol] + numberStr;
            leftCol--;
        }

        int rightCol = col + 1;
        while (rightCol < schematic[row].Length && char.IsDigit(schematic[row][rightCol]))
        {
            numberStr += schematic[row][rightCol];
            rightCol++;
        }

        return int.Parse(numberStr);
    }

    private bool IsValidSymbol(char symbol) => !char.IsDigit(symbol) && symbol != '.';

    private bool IsAdjacentToSymbol(int row, int col)
    {
        foreach (var (dx, dy) in DIRECTIONS)
        {
            if (0 <= row + dx && row + dx < schematic.Length &&
                0 <= col + dy && col + dy < schematic[row + dx].Length &&
                IsValidSymbol(schematic[row + dx][col + dy]))
            {
                return true;
            }
        }

        return false;
    }
}

class Program
{
    static void Main()
    {
        string filePath = "";

        try
        {
            string engineSchematic = File.ReadAllText(filePath);
            EngineSchematicAnalyzer analyzer = new EngineSchematicAnalyzer(engineSchematic);

            // Part 1
            long totalSumOfPartNumbers = analyzer.CalculateSumOfPartNumbers();
            Console.WriteLine($"Sum of part numbers is {totalSumOfPartNumbers}");

            // Part 2
            long totalSumOfGearRatios = analyzer.CalculateSumOfAllGearRatios();
            Console.WriteLine($"Sum of all gear ratios is {totalSumOfGearRatios}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
