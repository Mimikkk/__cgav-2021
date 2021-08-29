using System;
using System.Linq;
using Sokoban.Utilities.Extensions;

namespace Sokoban.Engine.Scripts
{
internal static class ScriptLoader
{
  static ScriptLoader() =>
    typeof(MonoBehaviour).Assembly.GetTypes()
      .Where(IsScript)
      .Select(CreateScript)
      .OrderBy(ScriptPriority)
      .ForEach(RunScript);

  private static LoadPriority ScriptPriority(MonoBehaviour script) => script.LoadPriority;
  private static bool IsScript(Type type) => type.IsSubclassOf(typeof(MonoBehaviour)) && !type.IsAbstract;
  private static MonoBehaviour CreateScript(Type type) => (MonoBehaviour)Activator.CreateInstance(type)!;
  private static void RunScript(MonoBehaviour script) => script.Run();
}
}