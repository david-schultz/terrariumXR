using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Note: Using a north-based coordinate system.
//Reference: https://youtu.be/bnmr_At2R0s?si=1SoFZTUouIKO4uDJ

public static class CubeMeshData {
    
    public static Vector3[] vertices = {
        //north-side
        new Vector3( 1,  1,  1),
        new Vector3(-1,  1,  1),
        new Vector3(-1, -1,  1),
        new Vector3( 1, -1,  1),
        //south-side
        new Vector3(-1,  1, -1),
        new Vector3( 1,  1, -1),
        new Vector3( 1, -1, -1),
        new Vector3(-1, -1, -1),
    };

    public static int[][] faceTriangles = {
        //north-side
        new int[] {0, 1, 2, 3},
        //east-side
        new int[] {5, 0, 3, 6},
        //south-side
        new int[] {4, 5, 6, 7},
        //west-side
        new int[] {1, 4, 7, 2},
        //top-side
        new int[] {5, 4, 1, 0},
        //bottom-side
        new int[] {3, 2, 7, 6},
    };

    public static Vector3[] faceVertices(int dir, float scale) {
        Vector3[] fv = new Vector3[4];
        for (int i = 0; i < fv.Length; i++) {
            Vector3 vertex = vertices[faceTriangles[dir][i]];
            fv[i] = Vector3.Scale(vertex, new Vector3(scale, scale, scale));
        }
        return fv;
    }

}
