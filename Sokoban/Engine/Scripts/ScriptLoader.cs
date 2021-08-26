using System;
using System.Linq;
using Sokoban.Utilities.Extensions;

namespace Sokoban.Engine.Scripts
{
internal static class ScriptLoader
{
  static ScriptLoader() => typeof(MonoBehaviour).Assembly.GetTypes().Where(IsScript).ForEach(RunScript);

  private static bool IsScript(Type type) => type.IsSubclassOf(typeof(MonoBehaviour)) && !type.IsAbstract;
  private static MonoBehaviour RunScript(Type type) => (MonoBehaviour)Activator.CreateInstance(type)!;
}
}