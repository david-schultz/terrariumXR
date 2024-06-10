using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralMeshes
{
    /// <summary>
    /// Holds a dictionary of Vertices. 
    /// </summary>
    [CreateAssetMenu(fileName="VertexDictionary_Data", menuName="Vertex Dictionary")]
    public class VertexDictionarySO : ScriptableObject
    {
        public Dictionary<string, GrabbableVertex> GrabbableVertices;
        
    }

    public class GrabbableVertex : MonoBehaviour
    {
        public int Index = -1;

        public Vector3 Direction;
        public Vector3 Position;
        public float MinDistance = 0.23f;
        public float MaxDistance = 0.3f;

        public bool isPinned = false;

    }
}
