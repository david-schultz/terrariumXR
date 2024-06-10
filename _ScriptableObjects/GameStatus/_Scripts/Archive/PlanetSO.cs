using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR.Geometry;

namespace TerrariumXR
{
    // Note: ScriptableObjects are not persistent between sessions in final build.
    // Tutorial to maintain persistence: https://www.youtube.com/watch?v=LLpBOj6p9Aw
    [CreateAssetMenu(fileName="Planet_Data", menuName="Planet")]
    public class PlanetSO : ScriptableObject
    {
    // ================== Variables ==================
        // These should be added during instantiation.
        public string Name;
        public Material PlanetMaterial;
        
        // These are optionally modified during instantiation.
        public float Radius = 0.25f;
        public int IcosphereSubdivisions = 1;
        public bool SmoothNormals = false;

        // These should be added after instantiation, during mesh generation.
        public List<Vector3> Vertices;
        public List<MeshTriangle> MeshTriangles;
    }
}