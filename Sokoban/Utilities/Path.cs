using System;
using System.IO;

namespace Sokoban.Utilities
{
public class Path
{
  public bool IsFile() => File.Exists(Str);
  public string LoadFileToString()
  {
    if (!IsFile()) throw new Exception($"File Loading from Path {Str} failed. Path is not pointing to File");
    return File.ReadAllText(Str);
  }
  public StreamReader LoadFileToStream()
  {
    if (!IsFile())
      throw new Exception($"Loading File Stream from Path {Str} failed. Path is not pointing to File");
    return new StreamReader(Str);
  }

  public Path(string path) => Str = path;
  public override string ToString() => Str;

  public static Path operator /(Path a, Path b) => new($"{a}/{b}");
  public static Path operator /(Path a, string b) => b switch {
    ".."   => new Path(Directory.GetParent(a.ToString())?.FullName ?? string.Empty),
    "../"  => new Path(Directory.GetParent(a.ToString())?.FullName ?? string.Empty),
    "..\\" => new Path(Directory.GetParent(a.ToString())?.FullName ?? string.Empty),
    "."    => a,
    "./"   => a,
    ".\\"  => a,
    _      => new Path($"{a}\\{b}")
  };

  private string Str { get; }
}
}