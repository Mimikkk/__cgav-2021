using System;

namespace Sokoban.Utilities
{
public class Filesystem
{
  private static readonly Path Working = new(Environment.CurrentDirectory);
  private static readonly Path Platform = Working / "..";
  private static readonly Path Bin = Platform / "..";
  private static readonly Path Project = Bin / "..";
  private static readonly Path Resources = Project / "resources";
  public static readonly Path Shaders = Resources / "shaders";
  public static readonly Path Textures = Resources / "textures";
  public static readonly Path Objects = Resources / "objects";
}}