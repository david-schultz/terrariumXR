using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrariumXR.Geometry
{
    public class MeshTriangle
    {
        public List<int>            VertexIndices;
        public List<Vector2>        UVs;
        public List<MeshTriangle>   Neighbours;
        public Color32 Color;
        public bool IsSelected;
        public int ExtrudeCount;
        public bool IsExtruded;
        public bool IsInset;
        public bool IsSide;
        public float ExtrudeDelta;

        public MeshTriangle(int _vertexIndexA, int _vertexIndexB, int _vertexIndexC)
        {
            VertexIndices = new List<int>() {_vertexIndexA,_vertexIndexB,_vertexIndexC};
            UVs = new List<Vector2>{Vector2.zero,Vector2.zero,Vector2.zero};
            Neighbours = new List<MeshTriangle>();
            IsSelected = false;
            ExtrudeCount = 0;
            IsExtruded = false;
            IsInset = false;
            IsSide = false;
            ExtrudeDelta = 0f;
        }

        /// <summary>
        /// Checks if a given MeshTriangle shares more than one vertex (i.e. an edge) with this MeshTriangle.
        /// </summary>
        public bool IsNeighbouring(MeshTriangle _other)
        {
            int sharedVertices = 0;
            foreach(int index in VertexIndices)
            {
                if(_other.VertexIndices.Contains(index))
                {
                    sharedVertices++;
                }
            }
            return sharedVertices > 1;
        }

        /// <summary>
        /// When a neighboring MeshTriangle is modified (i.e. recreated), this function is called to update the reference.
        /// </summary>
        public void UpdateNeighbour(MeshTriangle _initialNeighbour, MeshTriangle _newNeighbour)
        {
            for(int i = 0; i < Neighbours.Count; i++)
            {
                if(_initialNeighbour == Neighbours[i])
                {
                    Neighbours[i] = _newNeighbour;
                    return;
                }
            }
        }
    }
}