using System;
using System.IO;
using System.Text;
using Booster.CodingTest.Library;
using Moq;
using Xunit;
using TextStreamProcessor.Workers;

namespace TextStreamProcessorUnitTest;

public class StreamProcessorTests
{
    private readonly StreamProcessor _processor;

    public StreamProcessorTests()
    {
        _processor = new StreamProcessor();
    }

    [Fact]
    public void ProcessData_EmptyInput()
    {
        // Arrange
        byte[] buffer = new byte[0]; // Empty byte array

        // Act
        _processor.ProcessData(buffer, buffer.Length);

        // Assert
        Assert.Equal(0, _processor.TextDataModel.totalCharacters);
        Assert.Equal(0, _processor.TextDataModel.totalWords);
        Assert.Empty(_processor.TextDataModel.wordFrequency);
        Assert.Empty(_processor.TextDataModel.charFrequency);
    }

    [Fact]
    public void ProcessData_SingleWord()
    {
        // Arrange
        string text = "hello";
        byte[] buffer = Encoding.ASCII.GetBytes(text);

        // Act
        _processor.ProcessData(buffer, buffer.Length);

        // Assert
        Assert.Equal(5, _processor.TextDataModel.totalCharacters);
        Assert.Equal(1, _processor.TextDataModel.totalWords);
        Assert.Equal(1, _processor.TextDataModel.wordFrequency["hello"]);
        Assert.Equal(1, _processor.TextDataModel.charFrequency['h']);
        Assert.Equal(1, _processor.TextDataModel.charFrequency['e']);
        Assert.Equal(2, _processor.TextDataModel.charFrequency['l']);
        Assert.Equal(1, _processor.TextDataModel.charFrequency['o']);
    }

    [Fact]
    public void ProcessData_MultipleWords()
    {
        // Arrange
        string text = "This is a test";
        byte[] buffer = Encoding.ASCII.GetBytes(text);

        // Act
        _processor.ProcessData(buffer, buffer.Length);

        // Assert
        Assert.Equal(11, _processor.TextDataModel.totalCharacters);
        Assert.Equal(4, _processor.TextDataModel.totalWords);
        Assert.Equal(1, _processor.TextDataModel.wordFrequency["This"]);
        Assert.Equal(1, _processor.TextDataModel.wordFrequency["is"]);
        Assert.Equal(1, _processor.TextDataModel.wordFrequency["a"]);
        Assert.Equal(1, _processor.TextDataModel.wordFrequency["test"]);
        // Assert character frequencies for all characters
    }

    [Fact]
    public void ProcessData_WithPunctuation()
    {
        // Arrange
        string text = "Hello, world!";
        byte[] buffer = Encoding.ASCII.GetBytes(text);

        // Act
        _processor.ProcessData(buffer, buffer.Length);

        // Assert
        // Punctuation is considered a character
        Assert.Equal(12, _processor.TextDataModel.totalCharacters);

        Assert.Equal(2, _processor.TextDataModel.totalWords);
        Assert.Equal(1, _processor.TextDataModel.wordFrequency["Hello"]);
        Assert.Equal(1, _processor.TextDataModel.wordFrequency["world"]);
        // Assert character frequencies for all characters (including punctuation)
    }


}

