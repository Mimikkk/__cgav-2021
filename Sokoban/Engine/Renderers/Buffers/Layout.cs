using System.Collections.Generic;
using System.Linq;
using Logger;

namespace Sokoban.Engine.Renderers.Buffers
{
public readonly struct Layout
{
  public readonly uint Size;
  public readonly IReadOnlyList<int> Elements;

  public Layout(params int[] elements)
  {
    Size = (uint)(elements.Sum() * sizeof(float));
    Elements = elements;
  }
}}