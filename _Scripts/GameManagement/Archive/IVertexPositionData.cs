using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrariumXR.Interaction
{
    public interface IVertexPositionData
    {
        public int[] vertices {get; }
        public float delta { get; }
    }

    public class VertexPositionData : IVertexPositionData
    {
        public int[] vertices { get; }
        public float delta { get; }

        public VertexPositionData(int[] vertices, float delta)
        {
            this.vertices = vertices;
            this.delta = delta;
        }
    }
}