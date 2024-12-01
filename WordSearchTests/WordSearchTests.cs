using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class WordSearchTests
{
    [Fact]
    public void NullMatrix_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => new WordSearch(null));
    }

    [Fact]
    public void EmptyMatrix_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => new WordSearch(new List<string>()));
    }

    [Fact]
    public void MatrixExceedsMaxSize_ThrowsException()
    {
        var matrix = Enumerable.Repeat(new string('A', 65), 65);
        Assert.Throws<ArgumentException>(() => new WordSearch(matrix));
    }

    [Fact]
    public void ValidMatrix_EmptyWordStream_ReturnsEmpty()
    {
        var matrix = new List<string>
        {
            "chill",
            "cloud",
            "windy",
            "sharp",
            "frost"
        };

        var wordSearch = new WordSearch(matrix);
        var result = wordSearch.Find(new List<string>());
        Assert.Empty(result);
    }

    [Fact]
    public void FindWordsInMatrix_ReturnsCorrectWords_HorizontalAndVertical()
    {
        var matrix = new List<string>
        {
            "chill",
            "cloud",
            "windy",
            "sharp",
            "frost"
        };

        var wordStream = new List<string> { "chill", "wind", "cold", "frost" };

        var wordSearch = new WordSearch(matrix);
        var result = wordSearch.Find(wordStream).ToList();

        Assert.Equal(new List<string> { "chill", "wind", "frost" }.Order(), result);
    }

    [Fact]
    public void FindWordsInMatrix_LargeMatrix_HandlesEfficiently()
    {
        var matrix = Enumerable.Repeat(new string('A', 64), 64).ToList();
        matrix[0] = "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var wordStream = new List<string> { "ABCDE", "FGHIJ", "KLMNO", "QRSTU" };

        var wordSearch = new WordSearch(matrix);
        var result = wordSearch.Find(wordStream).ToList();

        Assert.Contains("ABCDE", result);
    }

    [Fact]
    public void MatrixDoesNotContainWords_ReturnsEmpty()
    {
        var matrix = new List<string>
        {
            "chill",
            "cloud",
            "windy",
            "sharp",
            "frost"
        };

        var wordStream = new List<string> { "sunny", "rain", "snow" };

        var wordSearch = new WordSearch(matrix);
        var result = wordSearch.Find(wordStream);

        Assert.Empty(result);
    }

    [Fact]
    public void DuplicateWordsInStream_CountsOnlyOnce()
    {
        var matrix = new List<string>
        {
            "chill",
            "cloud",
            "windy",
            "sharp",
            "frost"
        };

        var wordStream = new List<string> { "chill", "chill", "wind", "wind", "frost" };

        var wordSearch = new WordSearch(matrix);
        var result = wordSearch.Find(wordStream).ToList();

        Assert.Equal(new List<string> { "chill", "wind", "frost" }.Order(), result);
    }

    [Fact]
    public void LargeWordStream_PerformanceTest()
    {
        var matrix = new List<string>
        {
            "ABCDEFGHIJKL",
            "MNOPQRSTUVWX",
            "YZABCDEFGHIJ",
            "KLMNOPQRSTUV",
            "WXYZABCDEFGHI",
            "JKLMNOPQRSTUV"
        };

        var wordStream = Enumerable.Range(1, 10000).Select(x => "ABCD").ToList();
        var wordSearch = new WordSearch(matrix);

        var result = wordSearch.Find(wordStream).ToList();

        Assert.Contains("ABCD", result);
    }

    [Fact]
    public void SubstringWords_MatchCorrectly()
    {
        var matrix = new List<string>
        {
            "chill",
            "cloud",
            "windy",
            "sharp",
            "frost"
        };

        var wordStream = new List<string> { "chill", "cloud", "wind", "frost" };

        var wordSearch = new WordSearch(matrix);
        var result = wordSearch.Find(wordStream).ToList();

        Assert.Equal(new List<string> { "chill", "cloud", "wind", "frost" }.Order(), result);
    }

    [Fact]
    public void EdgeCase_SingleCharacterMatrix()
    {
        var matrix = new List<string>
        {
            "A"
        };

        var wordStream = new List<string> { "A", "B" };

        var wordSearch = new WordSearch(matrix);
        var result = wordSearch.Find(wordStream).ToList();

        Assert.Equal(new List<string> { "A" }, result);
    }

    [Fact]
    public void EdgeCase_SingleRowMatrix()
    {
        var matrix = new List<string>
        {
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        };

        var wordStream = new List<string> { "ABC", "XYZ", "PQR" };

        var wordSearch = new WordSearch(matrix);
        var result = wordSearch.Find(wordStream).ToList();

        Assert.Equal(new List<string> { "ABC", "XYZ", "PQR" }.Order(), result);
    }

    [Fact]
    public void EdgeCase_SingleColumnMatrix()
    {
        var matrix = new List<string>
        {
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J"
        };

        var wordStream = new List<string> { "ABC", "DEF", "GHI" };

        var wordSearch = new WordSearch(matrix);
        var result = wordSearch.Find(wordStream).ToList();

        Assert.Equal(new List<string> { "ABC", "DEF", "GHI" }, result);
    }

    [Fact]
    public void WordAppearsMultipleTimesInStream_CountsOnlyOnce()
    {
        var matrix = new List<string>
        {
            "appleorangebanana",
            "grapepeachpearplum",
            "mangokiwiguavaberry",
            "papayacherrylemon",
            "limewatermelonfig"
        };

        var wordStream = new List<string>
        {
            "apple", "orange", "apple", "banana", "apple", "grape", "orange", "banana",
            "mango", "apple", "kiwi"
        };

        var wordSearch = new WordSearch(matrix);
        var result = wordSearch.Find(wordStream).ToList();

        Assert.Equal(new List<string> { "apple", "orange", "banana", "grape", "mango", "kiwi" }.Order(), result);
    }

    [Fact]
    public void Over20WordsInWordStream_ReturnsTop10Results()
    {
        var matrix = new List<string>
        {
            "appleorangebanana",
            "grapepeachpearplum",
            "mangokiwiguavaberry",
            "papayacherrylemon",
            "limewatermelonfig"
        };

        var wordStream = new List<string>
        {
            "apple", "orange", "banana", "grape", "peach", "pear", "plum", "mango", "kiwi",
            "guava", "berry", "papaya", "cherry", "lemon", "lime", "watermelon", "fig",
            "orange", "banana", "cherry", "apple", "mango", "peach"
        };

        var wordSearch = new WordSearch(matrix);
        var result = wordSearch.Find(wordStream).ToList();

        var expected = new []{ "apple", "banana", "berry", "cherry", "fig", "grape", "guava", "kiwi", "lemon", "lime" };
        

        Assert.Equal(expected.Order(), result);
    }

    [Fact]
    public void WordAppearsMultipleTimesInMatrix_IsCountedOnce()
    {
        var matrix = new List<string>
        {
            "appleappleapple",
            "appleorangebanana",
            "appleappleapple",
            "grapepeachpearplum"
        };

        var wordStream = new List<string> { "apple", "orange", "banana", "grape", "peach" };

        var wordSearch = new WordSearch(matrix);
        var result = wordSearch.Find(wordStream).ToList();

        Assert.Equal(new List<string> { "apple", "orange", "banana", "grape", "peach" }.Order(), result);
    }

    [Fact]
    public void FindVerticalWords_ReturnsCorrectResults()
    {
        var matrix = new List<string>
        {
            "abcd",
            "efgh",
            "ijkl",
            "mnop"
        };

        var wordStream = new List<string>
        {
            "aeim", // Vertical word from column 0
            "bfjn", // Vertical word from column 1
            "cgko", // Vertical word from column 2
            "dhlp"  // Vertical word from column 3
        };

        var wordSearch = new WordSearch(matrix);
        var result = wordSearch.Find(wordStream).ToList();

        Assert.Equal(new List<string> { "aeim", "bfjn", "cgko", "dhlp" }, result);
    }

    [Fact]
    public void WordsWithCountsGreaterThanFour_AreIncludedCorrectly()
    {
        var matrix = new List<string>
        {
            "aaaa",
            "aaaa",
            "aaaa",
            "aaaa"
        };

        var wordStream = new List<string>
        {
            "a",      // Appears 32 times
            "aa",     // Appears 24 times
            "aaa",    // Appears 16 times
            "aaaa"    // Appears 8 times
        };

        var wordSearch = new WordSearch(matrix);
        var result = wordSearch.GetAllWordsWithCounts(); // Get all words and their counts

        var expected = new Dictionary<string, int>
        {
            { "a", 32 },
            { "aa", 24 },
            { "aaa", 16 },
            { "aaaa", 8 }
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public void WordsWithCountsGreaterThanFour_RealWordsIncludedCorrectly()
    {
        var matrix = new List<string>
        {
            "appleappleapple",
            "orangeorangeoran",
            "bananaappleorange",
            "appleorangebanana"
        };

        var wordStream = new List<string>
        {
            "apple",   // Appears multiple times
            "orange",  // Appears multiple times
            "banana",  // Appears multiple times
            "grape"    // Does not appear
        };

        var wordSearch = new WordSearch(matrix);
        var result = wordSearch.Find(wordStream).ToList();

        // Expected results: Words sorted by frequency, then alphabetically
        var expected = new List<string>
        {
            "apple",  // Highest frequency
            "orange", // Second highest frequency
            "banana"  // Third highest frequency
        };

        Assert.Equal(expected, result);
    }

}
