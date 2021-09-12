﻿using System.Linq;
using Logger;
using Silk.NET.Input;
using Silk.NET.Maths;
using Sokoban.Engine.Controllers;
using Sokoban.Engine.Objects;
using Sokoban.Engine.Objects.Primitives;
using Sokoban.Engine.Objects.Primitives.Textures;
using Sokoban.Engine.Scripts;
using ObjectLoader = Sokoban.Engine.Objects.Loaders.ObjectLoader;

namespace Sokoban.Scripts
{
public class GhostBehaviour : MonoBehaviour
{
  private static float HeightScale = 1f;

  protected override void Start()
  {
    Controller.OnHold(Key.T, dt => HeightScale = (float)(HeightScale + dt));
    Controller.OnHold(Key.G, dt => HeightScale = (float)(HeightScale - dt));

    Ghost.Spo = new("PBR") {
      Fragment = default,
      Vertex = default,
      ShouldLink = true,
    };

    Ghost.Mesh!.Material!.DiffuseMap = new Texture("Fabric/Color.png");
    Ghost.Mesh.Material.NormalMap = new Texture("Fabric/Normal.png");
    Ghost.Mesh.Material.DisplacementMap = new Texture("Fabric/Displacement.png");
    Ghost.Position = Vector3D<float>.One;
  }

  protected override void Render(double dt) => Ghost.Draw(() => {
    Ghost.Mesh!.Material!.DiffuseMap?.Bind(0);
    Ghost.Mesh.Material.NormalMap?.Bind(1);
    Ghost.Mesh.Material.DisplacementMap?.Bind(2);

    Ghost.Spo!.SetUniform("diffuse_map", 0);
    Ghost.Spo.SetUniform("normal_map", 1);
    Ghost.Spo.SetUniform("displacement_map", 2);
    
    Ghost.Spo.SetUniform("height_scale", HeightScale);
    Ghost.Spo.SetUniform("light_position", Camera.Position);
    Ghost.Spo.SetUniform("is_discardable", false);
    Ghost.Spo.SetUniform("model", Ghost.View);
  });

  private static readonly GameObject Ghost = ObjectLoader.Load("Ghost").First();
}
}