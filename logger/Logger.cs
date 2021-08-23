using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Logger
{
public static class Logger
{
  public static void Log(this string str, int depth = default) => Console.Write($"{HandleDepth(depth)}{HandleColors(str)}");
  public static void Log(this object obj, int depth = default) => obj.ToString().Log(depth);
  public static void LogLine(this string str, int depth = default) => $"{str}\n".Log(depth);
  public static void LogLine(this object obj, int depth = default) => obj.ToString().LogLine(depth);

  [DllImport("kernel32.dll")] private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);
  [DllImport("kernel32.dll")] private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
  [DllImport("kernel32.dll", SetLastError = true)]
  private static extern IntPtr GetStdHandle(int nStdHandle);
  [DllImport("kernel32.dll")] private static extern uint GetLastError();

  static Logger()
  {
    var iStdOut = GetStdHandle(StdOutputHandle);
    if (!GetConsoleMode(iStdOut, out var outConsoleMode))
    {
      Console.WriteLine("failed to get output console mode");
      Console.ReadKey();
      return;
    }
    outConsoleMode |= EnableVirtualTerminalProcessing;
    if (SetConsoleMode(iStdOut, outConsoleMode)) return;
    Console.WriteLine($"failed to set output console mode, error code: {GetLastError()}");
    Console.ReadKey();
  }

  private static readonly Regex ColorPattern = new(@"<c(\d+)\s*((.|\n)*?)\|>", RegexOptions.Multiline);
  private static string ColorCode(object code = null) => $"\u001b[38;5;{code ?? DefaultColorCode}m";
  private static string ColorCode(Match match) => $"{ColorCode(match.Groups[1])}{match.Groups[2]}{ColorCode()}";
  private static string HandleColors(string str) => ColorPattern.Replace(str, ColorCode);
  private static string HandleDepth(int depth) => $"{new string(' ', depth)}{(depth > 0 ? "- " : "")}";

  private const int StdOutputHandle = -0xB;
  private const uint EnableVirtualTerminalProcessing = 0x0004;
  private const uint DefaultColorCode = 7;
}
}