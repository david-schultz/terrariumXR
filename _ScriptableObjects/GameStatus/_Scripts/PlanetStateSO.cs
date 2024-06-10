using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR.Geometry;
using Octree;
using NumericsConverter;

namespace TerrariumXR
{
   // Note: ScriptableObjects are not persistent between sessions in final build.
   // Tutorial to maintain persistence: https://www.youtube.com/watch?v=LLpBOj6p9Aw
   [CreateAssetMenu(fileName="PlanetStateData", menuName="Global/PlanetStateData")]
   public class PlanetStateSO : ScriptableObject
   {
      public string Name;
      public float Radius = 0.25f;
      public int Subdivisions = 0;
      public bool SmoothNormals = false;
      public Color32 DefaultColor = new Color32(226, 208, 165, 0);

      public SimpleOctree Octree;
      public List<MeshTriangle> MeshTriangles;

      public void Initialize(string name)
      {
         Name = name;
         Generate();
      }

      public void Initialize(string name, float radius, int subdivisions, bool smoothNormals)
      {
         Name = name;
         Radius = radius;
         Subdivisions = subdivisions;
         SmoothNormals = smoothNormals;
         Generate();
      }

      private void Generate()
      {
         IcosphereGenerator generator = new IcosphereGenerator(Subdivisions, Radius, DefaultColor);
         Octree = generator.GetOctree();
         MeshTriangles = generator.GetMeshTriangles();
      }
   }
}