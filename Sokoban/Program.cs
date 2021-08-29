using System.Linq;
using Logger;
using Silk.NET.Maths;
using Sokoban.Engine.Renderers;
using Sokoban.Engine.Scripts;
using Sokoban.Utilities;
using App = Sokoban.Engine.Application.App;

namespace Sokoban
{
internal static class Program
{
  private static void Main()
  {
    Invoker.Static(typeof(ScriptLoader));
    App.Run();
  }
}
}