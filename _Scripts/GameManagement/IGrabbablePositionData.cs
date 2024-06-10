using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrariumXR.Interaction
{
    public interface IGrabbablePositionData
    {
        public int[] vertices {get; }
        public float delta { get; }
    }

    public class GrabbablePositionData : IGrabbablePositionData
    {
        public int[] vertices { get; }
        public float delta { get; }

        public GrabbablePositionData(int[] vertices, float delta)
        {
            this.vertices = vertices;
            this.delta = delta;
        }
    }
}