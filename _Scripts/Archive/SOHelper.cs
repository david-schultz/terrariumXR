using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ProceduralMeshes;

public static class SOHelper
{
    // public static ScriptableObject GetInstance<T>(string path) where T : UnityEngine.ScriptableObject
    // {
    //     // Check if an asset at the path is already created.
    //     var obj = AssetDatabase.LoadAssetAtPath<T>(path);

    //     // If not, create a new one.
    //     if (obj == null)
    //     {
    //         // Create an instance of the ScriptableObject "in memory."
    //         obj = ScriptableObject.CreateInstance<T>();
        
    //         // Bundle that instance into an asset file at the specified path.
    //         AssetDatabase.CreateAsset(obj, path);

    //         // Save the asset.
    //         AssetDatabase.SaveAssets();
    //     }

    //     return obj;
    // }
}