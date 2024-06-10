using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR.Geometry;
using Octree;
using NumericsConverter;
using Unity.VisualScripting;

namespace TerrariumXR
{
    /// <summary>
    /// A PlanetState holds a list of Vertices, MeshTriangles, and Props.
    /// These are used to apply changes to the planet's geometry or props.
    /// </summary>
    public class PlanetState
    {
        public string Name;
        public float Radius = 0.25f;
        public int Subdivisions = 2;
        public bool SmoothNormals = false;

        // These should be added after instantiation, during mesh generation.
        public SimpleOctree Octree { get; set; }
        // public List<Vector3> Vertices { get; set; }
        public List<MeshTriangle> MeshTriangles { get; set; }
        // public int[] Octants { get; set; }
        // public PointOctree<int> Octree; // int represents id
        // public List<GameProp> Props { get; set; }

        public PlanetState(string name)
        {
            Name = name;
            Octree = new SimpleOctree(Vector3.zero, 1f);
            // Vertices = new List<Vector3>();
            MeshTriangles = new List<MeshTriangle>();
            // Octants = new int[8];
        }

        public PlanetState(string name, float radius, int subdivisions)
        {
            Name = name;
            Radius = radius;
            Subdivisions = subdivisions;
            Octree = new SimpleOctree(Vector3.zero, 1f);
            // Vertices = new List<Vector3>();
            MeshTriangles = new List<MeshTriangle>();
            // Octants = new int[8];
        }

        public PlanetState Clone()
        {
            PlanetState clone = new PlanetState(Name, Radius, Subdivisions);
            // clone.SmoothNormals = SmoothNormals;
            // //TODO clone the octree
            // clone.Octree = Octree.Clone();
            // // clone.Vertices = new List<Vector3>(Vertices);
            // clone.MeshTriangles = new List<MeshTriangle>(MeshTriangles);
            // clone.Octants = (int[]) Octants.Clone();
            return clone;
        }



        public Vector3 GetTriangleMidPoint(int triangleId)
        {
            MeshTriangle t = MeshTriangles[triangleId];
            float x = ( Octree.Get(t.VertexIndices[0]).x +
                        Octree.Get(t.VertexIndices[1]).x + 
                        Octree.Get(t.VertexIndices[2]).x ) / 3f;
            float y = ( Octree.Get(t.VertexIndices[0]).y +
                        Octree.Get(t.VertexIndices[1]).y + 
                        Octree.Get(t.VertexIndices[2]).y ) / 3f;
            float z = ( Octree.Get(t.VertexIndices[0]).z +
                        Octree.Get(t.VertexIndices[1]).z + 
                        Octree.Get(t.VertexIndices[2]).z ) / 3f;

            // float x = ( Vertices[t.VertexIndices[0]].x +
            //             Vertices[t.VertexIndices[1]].x + 
            //             Vertices[t.VertexIndices[2]].x ) / 3f;
            // float y = ( Vertices[t.VertexIndices[0]].y +
            //             Vertices[t.VertexIndices[1]].y + 
            //             Vertices[t.VertexIndices[2]].y ) / 3f;
            // float z = ( Vertices[t.VertexIndices[0]].z +
            //             Vertices[t.VertexIndices[1]].z + 
            //             Vertices[t.VertexIndices[2]].z ) / 3f;

            return new Vector3(x, y, z);
        }

        /// <summary>
        ///   Returns a list of vertices in the given octant.
        ///   The given x, y, and z booleans indicate which octant to scan.
        ///   e.g. (true, false, false) will return all vertices in the +x -y -z octant,
        ///   including vertices on each axis.
        /// </summary>
        /// 
        ///   octant1 = +x +y +z
        ///   octant2 = -x +y +z
        ///   octant3 = -x +y -z
        ///   octant4 = +x +y -z
        ///   octant5 = +x -y +z
        ///   octant6 = -x -y +z
        ///   octant7 = -x -y -z
        ///   octant8 = +x -y -z
        ///   
        // public List<int> GetVerticesInOctant(int octant)
        // {
        //     return Octree.GetVerticesInOctant(octant);
        // }

        // public List<int> GetTrianglesInOctant(int octant)
        // {
        //     List<int> trianglesInOctant = new List<int>();


            
        //     List<int> vertices = Octree.GetVerticesInOctant(octant);

        //     return trianglesInOctant;
        // }
    }
}