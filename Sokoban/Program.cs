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
    
    // To
    Unit.Hey(4 switch {4 => Unit.Bye, _ => Unit.Bye});
    // Zamiast
    
    switch (4)
    {
      case 4: // OBSZAR KRYTYCZNY //
        break;
      default: // OBSZAR KRYTYCZNY //
    }
  }
}
}