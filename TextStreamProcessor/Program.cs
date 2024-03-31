using Booster.CodingTest.Library;
using TextStreamProcessor.Workers;

namespace TextStreamProcessor;

static class Program
{
    static void Main(string[] args)
    {
        var stream = new WordStream();
        var processor = new StreamProcessor();
        processor.ProcessStream(stream);
    }
}
