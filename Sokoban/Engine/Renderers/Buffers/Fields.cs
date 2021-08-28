using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Sokoban.Engine.Renderers.Buffers
{
public readonly struct Fields
{
  public readonly IReadOnlyDictionary<string, nint> OffsetByName;
  public readonly IReadOnlyDictionary<string, nuint> SizeByName;
  public readonly IReadOnlyList<string> Names;
  public readonly Layout Layout;

  public Fields(params (string, int)[] fields)
  {
    SizeByName = fields.ToImmutableDictionary(field => field.Item1, field => (nuint)field.Item2 * sizeof(float));

    var offset = 0;
    var offsetByName = new Dictionary<string, nint>();
    foreach (var (field, size) in fields)
    {
      offsetByName[field] = offset;
      offset += size * sizeof(float);
    }
    OffsetByName = offsetByName;

    Names = fields.Select(field => field.Item1).ToImmutableList();
    Layout = new Layout(fields.Select(field => field.Item2).ToArray());
  }
};
}