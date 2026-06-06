using System.Text;

namespace WordCountTool;

class Result
{
    private readonly string? _filePath;

    private readonly long _bytesCount; // -c option
    private readonly long _linesCount; // -l option
    private readonly long _wordsCount; // -w option
    private readonly long _charactersCount; // -m option

    private readonly IEnumerable<string> _inputOptions;

    public Result(
        long bytesCount,
        long linesCount,
        long wordsCount,
        long charactersCount,
        string? filePath,
        IEnumerable<string> inputOptions)
    {
        _filePath = filePath;
        _bytesCount = bytesCount;
        _linesCount = linesCount;
        _wordsCount = wordsCount;
        _charactersCount = charactersCount;
        _inputOptions = inputOptions;
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();

        // lines count
        if (_inputOptions.Contains("-l"))
        {
            stringBuilder.Append(_linesCount);
            stringBuilder.Append(" ");
        }

        // characters count
        if (_inputOptions.Contains("-m"))
        {
            stringBuilder.Append(_charactersCount);
            stringBuilder.Append(" ");
        }

        // words count
        if (_inputOptions.Contains("-w"))
        {
            stringBuilder.Append(_wordsCount);
            stringBuilder.Append(" ");
        }

        // bytes count
        if (_inputOptions.Contains("-c"))
        {
            stringBuilder.Append(_bytesCount);
            stringBuilder.Append(" ");
        }

        if (_filePath is not null)
        {
            stringBuilder.Append(_filePath);
        }

        return stringBuilder.ToString();
    }
}
