using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StellarBody
{
    public interface IStellarBody
    {
        int id { get; }
        int parentId { get; set; }
        string type { get; }
        string name { get; set; }
        // INotes notes { get; }
        // ICharacteristics characteristics { get; }
        void AddChild(int id);
        List<int> GetChildren();
    }

    public interface IGalaxy : IStellarBody
    {
        // List<int> GetStarIds();
        // IStar CreateStar();
        // void AddStar(int starId);
        // void DeleteStar(int starId);
    }

    public interface IStar : IStellarBody
    {
        // List<int> GetPlanetIds();
        // IPlanet CreatePlanet();
        // void AddPlanet(int planetId);
        // void DeletePlanet(int planetId);
    }

    public interface IPlanet : IStellarBody
    {
        // List<int> GetFlagIds();
        // IFlag CreateFlag();
        // void AddFlag(int flagId);
        // void DeleteFlag(int flagId);
    }

    public interface IFlag : IStellarBody
    {
        bool isComplete { get; }
        void MarkComplete();
        void MarkIncomplete();
    }
}
