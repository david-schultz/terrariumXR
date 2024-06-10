using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TerrariumXR
{
    /// <summary>
    /// Holds a dictionary of PlanetStates, each of which represents the current state of a unique planet.
    /// </summary>
    [CreateAssetMenu(fileName="PlanetDictionary_Data", menuName="Global/Planet Dictionary")]
    public class PlanetDictionarySO : ScriptableObject
    {
    // // ================== Variables ==================
    //     public Dictionary<int, PlanetState> Planets;
    //     public Dictionary<int, PlanetState> InitialGeometries;
    //     public int ActiveIndex = 0;
    //     public PlanetState ActivePlanet
    //     {
    //         get
    //         {
    //             return Planets[ActiveIndex];
    //         }
    //     }

    // // ================== Publically Accessible ==================
    //     public void SetActivePlanet(PlanetState givenState)
    //     {
    //         if (Planets == null)
    //         {
    //             Planets = new Dictionary<int, PlanetState>();
    //         }
    //         if (InitialGeometries == null)
    //         {
    //             InitialGeometries = new Dictionary<int, PlanetState>();
    //         }

    //         foreach (int key in Planets.Keys)
    //         {
    //             if (Planets[key].Name == givenState.Name)
    //             {
    //                 Planets[key] = givenState;
    //                 ActiveIndex = key;
    //                 return;
    //             }
    //         }
            
    //         int newKey = Planets.Count;
    //         Planets.Add(newKey, givenState);
    //         InitialGeometries.Add(newKey, givenState);
    //         ActiveIndex = newKey;
    //     }

    //     public void SetActivePlanet(string givenName)
    //     {
    //         if (Planets == null)
    //         {
    //             Planets = new Dictionary<int, PlanetState>();
    //         }

    //         foreach (int key in Planets.Keys)
    //         {
    //             if (Planets[key].Name == givenName)
    //             {
    //                 ActiveIndex = key;
    //                 return;
    //             }
    //         }
            
    //         int newKey = Planets.Count;
    //         PlanetState planetState = new PlanetState(givenName);
    //         Planets.Add(newKey, planetState);
    //         ActiveIndex = newKey;
    //     }
    }
}