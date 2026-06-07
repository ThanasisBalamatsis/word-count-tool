using System.Text;
using WordCountTool;

class Program
{
    static readonly HashSet<string> _validOptions = ["-c", "-l", "-w", "-m"];

    static async Task Main(string[] args)
    {
        HashSet<string> validOptions = []; 
        List<string> otherArgs = []; 

        foreach (var arg in args)
        {
            if (arg.StartsWith('-'))
            {
                if (_validOptions.Contains(arg))
                {
                    validOptions.Add(arg);
                }
                else
                {
                    Console.WriteLine("Provided option is invalid.");
                    return;
                }
            }
            else
            {
                otherArgs.Add(arg);
            }
        }

        int otherArgsCount = otherArgs.Count();
        string? filePath = null;

        if (otherArgsCount > 1)
        {
            Console.WriteLine("Provided arguments are invalid.");
            return;
        }
        else if (otherArgsCount == 1)
        {
            var otherArg = otherArgs.FirstOrDefault();

            if (otherArg != args.LastOrDefault())
            {
                Console.WriteLine("Provided arguments order is invalid. Valid format: mywc [OPTION]... [FILE]...");
                return;
            }

            filePath = otherArg;

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Provided file path is not found.");
                return;
            }
        }

        var options = validOptions.Any()
            ? validOptions
            : ["-c", "-w", "-l"];

        try
        {
            await ExecuteWordCount(options, filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error occurred: {ex.Message}");
        }
    }

    static async Task ExecuteWordCount(
        IEnumerable<string> options,
        string? filePath)
    {
        Stream inputStream;

        if (string.IsNullOrEmpty(filePath))
        {
            inputStream = Console.OpenStandardInput();
        }
        else
        {
            inputStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        }

        using var stream = inputStream;

        var encoding = Encoding.Default;
        var decoder = encoding.GetDecoder();

        byte[] byteBuffer = new byte[4096];
        char[] charBuffer = new char[4096];
        int bytesRead;
        int charsDecoded;

        long bytesCount = 0;
        long linesCount = 0;
        long wordsCount = 0;
        bool inWord = false;
        long charactersCount = 0;

        while ((bytesRead = await stream.ReadAsync(byteBuffer, 0, byteBuffer.Length)) > 0)
        {
            bytesCount += bytesRead;

            charsDecoded = decoder.GetChars(
                byteBuffer,
                0,
                bytesRead,
                charBuffer,
                0);

            charactersCount += charsDecoded;

            for (int i = 0; i < charsDecoded; i++)
            {
                char character = charBuffer[i];

                if (character == '\n')
                {
                    linesCount++;
                }

                if (char.IsWhiteSpace(character))
                {
                    inWord = false;
                }
                else
                {
                    if (!inWord)
                    {
                        wordsCount++;
                        inWord = true;
                    }
                }
            }
        }

        var result = new Output(
            bytesCount: bytesCount,
            linesCount: linesCount,
            wordsCount: wordsCount,
            charactersCount: charactersCount,
            filePath: filePath,
            inputOptions: options);

        Console.WriteLine(result.ToString());
    }
}