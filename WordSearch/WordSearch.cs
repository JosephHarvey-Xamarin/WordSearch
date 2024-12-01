using System;
using System.Collections.Generic;
using System.Linq;

public class WordSearch
{
    private readonly Dictionary<string, int> _matrixWords;

    public WordSearch(IEnumerable<string> matrix)
    {
        if (matrix == null || !matrix.Any())
            throw new ArgumentException("Matrix cannot be null or empty.");

        var matrixArray = matrix.Select(row => row.ToCharArray()).ToArray();
        var rows = matrixArray.Length;
        var cols = matrixArray[0].Length;

        if (rows > 64 || cols > 64)
            throw new ArgumentException("Matrix size cannot exceed 64x64.");

        _matrixWords = new Dictionary<string, int>();

        // Extract horizontal words
        foreach (var row in matrixArray)
        {
            foreach (var word in GetAllSubstrings(new string(row)))
            {
                _matrixWords[word] = _matrixWords.GetValueOrDefault(word) + 1;
            }
        }

        // Extract vertical words
        for (int col = 0; col < cols; col++)
        {
            var verticalWord = new char[rows];
            for (int row = 0; row < rows; row++)
                verticalWord[row] = matrixArray[row][col];

            foreach (var word in GetAllSubstrings(new string(verticalWord)))
            {
                _matrixWords[word] = _matrixWords.GetValueOrDefault(word) + 1;
            }
        }
    }

    public Dictionary<string, int> GetAllWordsWithCounts()
    {
        return _matrixWords
            .OrderByDescending(x => x.Value) // Order by frequency
            .ThenBy(x => x.Key)             // Then alphabetically
            .ToDictionary(x => x.Key, x => x.Value);
    }

    private IEnumerable<string> GetAllSubstrings(string input)
    {
        var substrings = new List<string>();
        for (int start = 0; start < input.Length; start++)
        {
            for (int length = 1; length <= input.Length - start; length++)
            {
                substrings.Add(input.Substring(start, length));
            }
        }
        return substrings;
    }


    public IEnumerable<string> Find(IEnumerable<string> wordStream)
    {
        if (wordStream == null || !wordStream.Any())
            return Enumerable.Empty<string>();

        var wordCounts = new Dictionary<string, int>();

        foreach (string word in wordStream.Distinct()) // Ensure each word in the stream is processed only once
        {
           
                if (_matrixWords.ContainsKey(word))
                {
                    wordCounts[word] = _matrixWords[word]; // Use the frequency from the matrix
                }
        }

        return wordCounts
            .OrderByDescending(x => x.Value) // Order by frequency
            .ThenBy(x => x.Key)             // Then alphabetically
            .Take(10)                       // Take the top 10 results
            .Select(x => x.Key);            // Return only the words
    }

}
