using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//  octant1 = +x +y +z
//  octant2 = -x +y +z
//  octant3 = -x +y -z
//  octant4 = +x +y -z
//  octant5 = +x -y +z
//  octant6 = -x -y +z
//  octant7 = -x -y -z
//  octant8 = +x -y -z

namespace TerrariumXR
{
    public class SimpleOctree
    {
    // ================== Public Variables ==================
        // public int Count {
        //     get {
        //         return _vertices[0].Count;
        //         // return _dictionary.Count;
        //     }
        // }
        public int Count;

    // // ================== Tuple ==================
    //     private struct Tuple<T1, T2>
    //     {
    //         public readonly T1 Item1;
    //         public readonly T2 Item2;
    //         public Tuple(T1 item1, T2 item2) { Item1 = item1; Item2 = item2;} 
    //     }

    //     private static class Tuple // for type-inference goodness.
    //     { 
    //         public static Tuple<T1,T2> Create<T1,T2>(T1 item1, T2 item2)
    //         { 
    //             return new Tuple<T1,T2>(item1, item2); 
    //         }
    //     }

    // ================== Variables ==================
        // private List<Vector3>[] _vertices;
        private Dictionary<int, Vector3>[] _vertices;
        // private Dictionary<int, Tuple<int, int>> _overlap;
        private Bounds[] _bounds;

    // ================== Constructor ==================
        public SimpleOctree(Vector3 center, float width)
        {
            Count = 0;

            // Initialize the vertices and octants containers
            _vertices = new Dictionary<int, Vector3>[9];
            for (int i = 0; i < _vertices.Length; i++) // The 0th bound captures the entire octree
            {
                _vertices[i] = new Dictionary<int, Vector3>();
            }

            // _overlap = new Dictionary<int, Tuple<int, int>>();
            // _overlap.Add(0, Tuple.Create(1, 2));

            // Initialize bounds
            _bounds = new Bounds[9];
            _bounds[0] = new Bounds(center, new Vector3(width, width, width)); // The 0th bound captures the entire octree

            width = (width / 2) + 0.1f; // add a little extra to the width to prevent floating point errors
            float quarter = width / 4f;
            Vector3 _octantSize = new Vector3(width, width, width);
                
            _bounds[1] = new Bounds(center + new Vector3( quarter,  quarter,  quarter), _octantSize);
            _bounds[2] = new Bounds(center + new Vector3(-quarter,  quarter,  quarter), _octantSize);
            _bounds[3] = new Bounds(center + new Vector3(-quarter,  quarter, -quarter), _octantSize);
            _bounds[4] = new Bounds(center + new Vector3( quarter,  quarter, -quarter), _octantSize);
            _bounds[5] = new Bounds(center + new Vector3( quarter, -quarter,  quarter), _octantSize);
            _bounds[6] = new Bounds(center + new Vector3(-quarter, -quarter,  quarter), _octantSize);
            _bounds[7] = new Bounds(center + new Vector3(-quarter, -quarter, -quarter), _octantSize);
            _bounds[8] = new Bounds(center + new Vector3( quarter, -quarter, -quarter), _octantSize);
        }

    // ================== Public Functions ==================
        /// <summary>
        /// Add a vertex to the octree.
        /// </summary>
        /// <returns>The id of the newly added vertex.</returns>
        public int Add(Vector3 position)
        {
            // Check if the position is within the bounds of the octree
            if (!_bounds[0].Contains(position))
            {
                return -1;
            }

            if (!_vertices[0].ContainsValue(position))
            {
                _vertices[0].Add(Count, position);
            }

            foreach (int i in CalculateOctants(position))
            {
                _vertices[i].Add(Count, position);
            }

            Count++;
            return Count - 1;
            // return _vertices[0].Count - 1;
        }

        public Vector3 Get(int id)
        {
            return _vertices[0][id];
        }

        public void Adjust(int id, Vector3 position)
        {
            if (id == -1) return;

            if (_vertices[0].ContainsKey(id))
            {
                _vertices[0][id] = position;
            }

            foreach (int i in CalculateOctants(position))
            {
                _vertices[i][id] = position;
            }
        }

        public Dictionary<int, Vector3> GetOctant(int octant)
        {
            return _vertices[octant];
        }

        public Dictionary<int, Vector3> GetAll()
        {
            return _vertices[0];
        }

        public void PrintOctants()
        {
            Debug.Log("---- Current Octant Counts ----");
            for (int i = 1; i < _vertices.Length; i++)
            {
                Debug.Log("#" + i + ": " + _vertices[i].Count);
            }
        }

        public bool IsNeighbor(int id1, int id2)
        {
            // Vector3 pos1 = _vertices[0][id1];
            // Vector3 pos2 = _vertices[0][id2];
            // return Vector3.Distance(pos1, pos2) < 0.1f;
            return true;
        }

    // ================== Private Functions ==================
        public List<int> CalculateOctants(Vector3 position)
        {
            List<int> activeOctants = new List<int>();
            for (int i = 1; i < _bounds.Length; i++)
            {
                if (_bounds[i].Contains(position))
                {
                    activeOctants.Add(i);
                }
            }
            return activeOctants;
        }

        // private bool[] CalculateOctantsAsBool(Vector3 position)
        // {
        //     // index 0 is not used
        //     bool[] octants = {true, false, false, false, false, false, false, false, false};
        //     for (int i = 1; i < _bounds.Length; i++)
        //     {
        //         if (_bounds[i].Contains(position))
        //         {
        //             octants[i] = true;
        //         }
        //     }
        //     return octants;
        // }

        // /// <summary>
        // /// Root node of the octree
        // /// </summary>
        // private SimpleOctreeNode _rootNode;
        // private float _initialSize;
        // private float _minSize;
        // public int Count;

        // /// <summary>
        // /// Constructor for the point octree.
        // /// </summary>
        // /// <param name="initialWorldSize">Size of the sides of the initial node. The octree will never shrink smaller than this.</param>
        // /// <param name="initialWorldPos">Position of the centre of the initial node.</param>
        // /// <param name="minNodeSize">Nodes will stop splitting if the new nodes would be smaller than this.</param>
        // /// <exception cref="ArgumentException">Minimum node size must be at least as big as the initial world size.</exception>
        // public SimpleOctree(float initialWorldSize, Vector3 initialWorldPos, float minNodeSize)
        // {
        //     if (minNodeSize > initialWorldSize)
        //         throw new ArgumentException("Minimum node size must be at least as big as the initial world size.",
        //             nameof(minNodeSize));

        //     Count = 0;
        //     _initialSize = initialWorldSize;
        //     _minSize = minNodeSize;
        //     _rootNode = new SimpleOctreeNode(_initialSize, _minSize, initialWorldPos);
        // }


        // public void Add(int id, Vector3 position)
        // {
        //     _rootNode.Add(id, position);
        // }

        // public SimpleOctreeNode GetPosition(int id, Vector3 position)
        // {
        //     return _rootNode.GetPosition(id, position);
        // }

        // public void UpdatePosition(int id, Vector3 position)
        // {
        //     _rootNode.UpdatePosition(id, position);
        // }

        // public ICollection<SimpleOctreeNode> GetAll()
        // {
        //     List<SimpleOctreeNode> objects = new List<SimpleOctreeNode>(Count);
        //     _rootNode.GetAll(objects);
        //     return objects;
        // }

        // public ICollection<SimpleOctreeNode> GetVerticesInOctant(int octant)
        // {
        //     List<SimpleOctreeNode> objects = new List<SimpleOctreeNode>();
        //     _rootNode.GetVerticesInOctant(objects);
        //     return objects;
        // }
    }
}