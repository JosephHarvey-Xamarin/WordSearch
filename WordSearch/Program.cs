Console.WriteLine("Welcome to the Word Search Console App!");

// Input the matrix
Console.WriteLine("Enter the matrix row by row (type 'END' to finish):");
var matrix = new List<string>();
while (true)
{
    var input = Console.ReadLine();
    if (input == null)
    { continue; }

    if (input.Equals("END", StringComparison.OrdinalIgnoreCase))
        break;

    matrix.Add(input);
}

// Input the word stream
Console.WriteLine("Enter the words in the word stream separated by spaces:");
var wordStreamInput = Console.ReadLine();
if (wordStreamInput == null)
    return;
var wordStream = wordStreamInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);

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
