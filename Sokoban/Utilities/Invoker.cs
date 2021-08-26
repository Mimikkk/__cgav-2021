using System;
using System.Runtime.CompilerServices;

namespace Sokoban.Utilities
{
public static class Invoker
{
  public static void Static(Type type) => RuntimeHelpers.RunClassConstructor(type.TypeHandle);
}
}