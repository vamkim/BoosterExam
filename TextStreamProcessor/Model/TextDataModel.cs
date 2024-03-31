using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextStreamProcessor.Model;
public class TextDataModel
{
    public int totalCharacters { get; set; }
    public int totalWords { get; set; }
    public Dictionary<string, int> wordFrequency { get; set; } 
    public Dictionary<char, int> charFrequency { get; set; }

    public TextDataModel()
    {
        totalCharacters = 0;
        totalWords = 0;
        wordFrequency = new Dictionary<string, int>();
        charFrequency = new Dictionary<char, int>();
    }
}


