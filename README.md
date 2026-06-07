# mywc — Word Count Tool

A streaming C# / .NET 10 implementation of the Unix `wc` command, built as part of John Crickett's [Coding Challenges](https://codingchallenges.fyi/challenges/challenge-wc).

## Options

| Option | Description |
|--------|-------------|
| `-c`   | Number of bytes |
| `-l`   | Number of lines |
| `-w`   | Number of words |
| `-m`   | Number of characters (encoding-aware) |

No option defaults to `-c -l -w`.

## Build & Install

**Prerequisites:** [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Linux

```bash
dotnet publish -c Release -r linux-x64 --self-contained -o ./out
cp ./out/mywc /usr/local/bin/mywc
```

### Windows

```powershell
dotnet publish -c Release -r win-x64 --self-contained -o ./out
# Add the ./out directory to your PATH, or move mywc.exe to a directory already on it
```

## Usage

```bash
mywc [OPTION]... [FILE]
```

When no `FILE` is given, reads from standard input.

## Examples

```bash
$ mywc -c test.txt
342190 test.txt

$ mywc -l test.txt
7145 test.txt

$ mywc -w test.txt
58164 test.txt

$ mywc -m test.txt
339292 test.txt

$ mywc test.txt
7145 58164 342190 test.txt

$ cat test.txt | mywc -l
7145
```

## Testing

A `test.txt` file is included in the solution folder. Use it to verify your output matches the examples above.
