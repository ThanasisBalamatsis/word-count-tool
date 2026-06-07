using System.Text;
using WordCountTool;

class Program
{
    static readonly HashSet<string> _validOptions = ["-c", "-l", "-w", "-m"];
    static async Task Main(string[] args)
    {
        var validOptions = args.Where(_validOptions.Contains);
        var options = validOptions.Any()
            ? validOptions
            : ["-c", "-w", "-l"];

        var filePath = args.FirstOrDefault(a => !a.StartsWith('-'));

        if (filePath is null)
        {
            await ExecuteWordCount(options, filePath: null);
            return;
        }

        if (!File.Exists(filePath))
        {
            Console.WriteLine("Provided file path is not found.");
            return;
        }

        await ExecuteWordCount(options, filePath);
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

            for (int i = 0; i < bytesRead; i++)
            {
                if (byteBuffer[i] == (byte)'\n')
                {
                    linesCount++;
                }
            }

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

        var result = new Result(
            bytesCount: bytesCount,
            linesCount: linesCount,
            wordsCount: wordsCount,
            charactersCount: charactersCount,
            filePath: filePath,
            inputOptions: options);

        Console.WriteLine(result.ToString());
    }
}