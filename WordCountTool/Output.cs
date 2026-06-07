using System.Text;

namespace WordCountTool;

class Output
{
    private readonly string? _filePath;

    private readonly long _bytesCount;
    private readonly long _linesCount;
    private readonly long _wordsCount;
    private readonly long _charactersCount;

    private readonly HashSet<string> _inputOptions;

    public Output(
        long bytesCount,
        long linesCount,
        long wordsCount,
        long charactersCount,
        string? filePath,
        HashSet<string> inputOptions)
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
        if (_inputOptions.Contains(Options.LinesCount))
        {
            stringBuilder.Append(_linesCount);
            stringBuilder.Append(" ");
        }

        // characters count
        if (_inputOptions.Contains(Options.CharactersCount))
        {
            stringBuilder.Append(_charactersCount);
            stringBuilder.Append(" ");
        }

        // words count
        if (_inputOptions.Contains(Options.WordsCount))
        {
            stringBuilder.Append(_wordsCount);
            stringBuilder.Append(" ");
        }

        // bytes count
        if (_inputOptions.Contains(Options.BytesCount))
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
