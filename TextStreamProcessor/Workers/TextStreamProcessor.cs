using System.Diagnostics.CodeAnalysis;
using System.Text;
using Booster.CodingTest.Library;
using TextStreamProcessor.Model;

namespace TextStreamProcessor.Workers;
public class StreamProcessor
{
    private readonly TextDataModel _textDataModel;

    public TextDataModel TextDataModel => _textDataModel;
      
    public StreamProcessor()
    {
        _textDataModel = new TextDataModel();
    }
    private readonly StringBuilder characterCollection = new StringBuilder();

    public void ProcessStream(WordStream stream)
    {

        // Read from the stream and process in real time
        using (stream)
        {
            byte[] buffer = new byte[1024];
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                ProcessData(buffer, bytesRead);
                DisplayTotalCounts();
                WaitForKeyPress();
                DisplayLargestWords();
                WaitForKeyPress();
                DisplaySmallestWords();
                WaitForKeyPress();
                DisplayMostFrequentWords();
                WaitForKeyPress();
                DisplayCharacterFrequency();
                WaitForKeyPress();
                Console.WriteLine("------------------------------------");
            }
        }
    }

    private void GatherCharacterData(char letterPerLine)
    {
        _textDataModel.charFrequency.TryAdd(letterPerLine, 0);
        _textDataModel.charFrequency[letterPerLine]++;
    }
    private void GatherWordData(char letterPerLine)
    {
        // End of word
        string word = characterCollection.ToString();
        if (!string.IsNullOrWhiteSpace(word))
        {
            _textDataModel.totalWords++;
            _textDataModel.totalCharacters += word.Length;

            // Update word frequency dictionary
            _textDataModel.wordFrequency.TryAdd(word, 0);
            _textDataModel.wordFrequency[word]++;

            if (char.IsPunctuation(letterPerLine))
            {
                // Update character frequency dictionary
                _textDataModel.totalCharacters++;
                GatherCharacterData(letterPerLine);
            }

            // Clear StringBuilder for next word
            characterCollection.Clear();
        }
    }

    public void ProcessData(byte[] buffer, int bytesRead)
    {
        for (int i = 0; i < bytesRead; i++)
        {
            char letterPerLine = (char)buffer[i];
            if (char.IsWhiteSpace(letterPerLine) || char.IsPunctuation(letterPerLine))
            {
                GatherWordData(letterPerLine);
            }
            else
            {
                // Append character to StringBuilder
                characterCollection.Append(letterPerLine);

                // Update character frequency dictionary
                GatherCharacterData(letterPerLine);
            }
            
            // If this is the last byte read, gather the word data
            if (i == bytesRead - 1)
            {
                GatherWordData(letterPerLine);
            }
        }
    }

    [ExcludeFromCodeCoverage]
    private void DisplayTotalCounts()
    {
        Console.Clear();
        Console.WriteLine($"Total characters: {_textDataModel.totalCharacters}, Total words: {_textDataModel.totalWords}");
    }

    [ExcludeFromCodeCoverage]
    private void DisplayLargestWords()
    {
        Console.Clear();
        Console.WriteLine("Largest words:");
        var largestWords = _textDataModel.wordFrequency.OrderByDescending(pair => pair.Key.Length).Take(5);
        foreach (var pair in largestWords)
        {
            Console.WriteLine($"{pair.Key}: {pair.Value}");
        }
    }

    [ExcludeFromCodeCoverage]
    private void DisplaySmallestWords()
    {
        Console.Clear();
        Console.WriteLine("Smallest words:");
        var smallestWords = _textDataModel.wordFrequency.OrderBy(pair => pair.Key.Length).Take(5);
        foreach (var pair in smallestWords)
        {
            Console.WriteLine($"{pair.Key}: {pair.Value}");
        }
    }

    [ExcludeFromCodeCoverage]
    private void DisplayMostFrequentWords()
    {
        Console.Clear();
        Console.WriteLine("Most frequently appearing words:");
        var mostFrequentWords = _textDataModel.wordFrequency.OrderByDescending(pair => pair.Value).Take(10);
        foreach (var pair in mostFrequentWords)
        {
            Console.WriteLine($"{pair.Key}: {pair.Value}");
        }
    }

    [ExcludeFromCodeCoverage]
    private void DisplayCharacterFrequency()
    {
        Console.Clear();
        Console.WriteLine("Character frequency:");
        var sortedCharFrequency = _textDataModel.charFrequency.OrderByDescending(pair => pair.Value);
        foreach (var pair in sortedCharFrequency)
        {
            Console.WriteLine($"{pair.Key}: {pair.Value}");
        }
    }

    [ExcludeFromCodeCoverage]
    private void WaitForKeyPress()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}