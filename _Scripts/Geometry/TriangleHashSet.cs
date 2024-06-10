using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrariumXR.Geometry
{
    /// <summary>
    /// A TriangleHashSet contains a set of triangles, which represents some arbitrary grouping.
    /// e.g. a continent
    /// </summary>
    public class TriangleHashSet : HashSet<MeshTriangle>
    {
        public TriangleHashSet() {}
        public TriangleHashSet(TriangleHashSet source) : base(source) {}
        public int IterationIndex = -1;

        public BorderHashSet CreateBorderHashSet()
        {
            BorderHashSet borderSet = new BorderHashSet();
            foreach (MeshTriangle triangle in this)
            {
                foreach (MeshTriangle neighbor in triangle.Neighbours)
                {
                    if (this.Contains(neighbor))
                    {
                        continue;
                    }
                    TriangleBorder border = new TriangleBorder(triangle, neighbor);
                    borderSet.Add(border);
                }
            }
            return borderSet;
        }

        public List<int> ExcludeDuplicates()
        {
            List<int> vertices = new List<int>();
            foreach (MeshTriangle triangle in this)
            {
                foreach (int vertexIndex in triangle.VertexIndices)
                {
                    if (!vertices.Contains(vertexIndex))
                    {
                        vertices.Add(vertexIndex);
                    }      
                }
            }
            return vertices;
        }

        public void ApplyColor(Color32 _color)
        {
            foreach (MeshTriangle triangle in this)
                triangle.Color = _color;
        }
    }
}