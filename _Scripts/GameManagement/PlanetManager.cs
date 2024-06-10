using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static Debugger;
using TerrariumXR.EventSystem;
using TerrariumXR.Geometry;

namespace TerrariumXR
{
    public class PlanetManager : MonoBehaviour
    {
        [SerializeField] private Debugger _debugger;
        [SerializeField] private PlanetDictionarySO _dictionary;
        [SerializeField] private StringEventChannelSO _newPlanetChannel;


    // ================== Initialization ==================
        // void Start()
        // {
        //     _debugger.Log("PlanetManager started.");

        //     _dictionary.SetActivePlanet("Timber Hearth");
        //     _newPlanetChannel.RaiseEvent();

        //     _debugger.Log("Loaded " + _dictionary.ActivePlanet.Name + ", with " + _dictionary.ActivePlanet.Vertices.Count + " vertices.");
        // }

    // // ================== Update planet/apply state ==================
    //     public void UpdatePlanet(IGameState gameState)
    //     {
    //         // 1. Check what has changed between states

    //         ///////////
    //         ///// Below was auto-generated
    //         ///// • Update the planet's geometry.
    //         ///// • Update the mesh.
    //         ///// • No need to update MeshTriangles, as they are references to vertices.

    //         // List<int> indices = gameState.Indices;
    //         // List<Vector3> vertices = _planetSO.Vertices;

    //         // for (int i = 0; i < indices.Count; i++)
    //         // {
    //         //     vertices[indices[i]] = gameState.Position;
    //         // }

    //         // _mesh.GenerateMesh(_planetSO);
    //     }
    }
}