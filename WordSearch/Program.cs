using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Word Search Console App!");

        // Input the matrix
        var matrix = new List<string>();
        while (true)
        {
            Console.WriteLine("Enter the matrix row by row (type 'END' to finish):");
            matrix.Clear(); // Clear previous attempts
            bool validMatrix = true;

            while (true)
            {
                var input = Console.ReadLine();
                if (input == null)
                {
                    continue;
                }

                if (input.Equals("END", StringComparison.OrdinalIgnoreCase))
                    break;

                // Validate row length
                if (matrix.Count > 0 && input.Length != matrix[0].Length)
                {
                    Console.WriteLine("Error: All rows in the matrix must have the same length. Please try again.");
                    validMatrix = false;
                    break;
                }

                matrix.Add(input);
            }

            if (validMatrix && matrix.Count > 0)
                break; // Exit matrix input loop if valid
        }

        // Input the word stream
        string[] wordStream = null;
        while (true)
        {
            Console.WriteLine("Enter the words in the word stream separated by spaces:");
            var wordStreamInput = Console.ReadLine();
            if (wordStreamInput == null || string.IsNullOrWhiteSpace(wordStreamInput))
            {
                Console.WriteLine("Error: Word stream cannot be empty. Please try again.");
                continue;
            }

            wordStream = wordStreamInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (wordStream.All(w => !string.IsNullOrWhiteSpace(w)))
                break; // Valid word stream
            else
                Console.WriteLine("Error: Word stream contains invalid entries. Please try again.");
        }

        // Process and find matching words
        try
        {
            var wordSearch = new WordSearch(matrix);
            var result = wordSearch.Find(wordStream);

            Console.WriteLine("\nTop 10 Words Found:");
            foreach (var word in result)
            {
                Console.WriteLine(word);
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
