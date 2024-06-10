using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR.Geometry;

namespace TerrariumXR
{
    /// <summary>  
    ///  IGameState basically works by holding all the different aspects of the game;
    ///  but in practice, most of these aspects will be null.
    ///  This way, the command listener can check which aspects of the game should be updated, and updates only those.
    ///  
    ///  Example use cases:
    ///  • Select a list of vertices.
    ///  • Change the position of a list of vertices.
    ///  • Extrude a TriangleHashSet.
    /// </summary>
    public class GameState
    {
        // public string Gamemode { get; set; }
        public PlanetState PlanetState { get; }
        public List<int> SelectedVertices { get; }
        public List<int> SelectedTriangles { get; }
        public TriangleHashSet Selection { get; }

        public GameState(PlanetState planetState)
        {
            PlanetState = planetState;
            SelectedVertices = new List<int>();
            SelectedTriangles = new List<int>();
            Selection = new TriangleHashSet();
        }
        public Dictionary<string, bool> Menus { get; set; }

        public GameState(PlanetState planetState, List<int> selectedVertices, List<int> selectedTriangles)
        {
            PlanetState = planetState;
            SelectedVertices = selectedVertices;
            SelectedTriangles = selectedTriangles;
        }


        

        public GameState Clone()
        {
            return new GameState(PlanetState.Clone(), SelectedVertices, SelectedTriangles);
        }

        public GameState Clone(PlanetState newPlanetState)
        {
            return new GameState(newPlanetState, SelectedVertices, SelectedTriangles);
        }

        // public GameState Clone(List<int> newVertices)
        // {
        //     return new GameState(Gamemode, PlanetState, newVertices, SelectedTriangles);
        // }

        // public GameState Clone(List<int> newTriangles)
        // {
        //     return new GameState(Gamemode, PlanetState, SelectedVertices, newTriangles);
        // }
    }
}
