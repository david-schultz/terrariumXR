
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Oculus.Interaction;

public class Debugger : MonoBehaviour
{
// ================== Events ==================
    // public delegate void VertexGrabbed(GameObject source);
    // public event VertexGrabbed onVertexGrabbed;
    // public delegate void VertexMoved(GameObject source);
    // public event VertexMoved onVertexMoved;
    // public delegate void VertexReleased(GameObject source);
    // public event VertexReleased onVertexReleased;

    //         onVertexMoved?.Invoke(gameObject);


// ================== References ==================

    [SerializeField] private bool enableDebug = true;
    [SerializeField] private int maxLines = 15;

    private Dictionary<string, TextMeshProUGUI> titles;
    private Dictionary<string, TextMeshProUGUI> datums;

    private bool initialized = false;

    void Awake()
    {
        titles = new Dictionary<string, TextMeshProUGUI>();
        datums = new Dictionary<string, TextMeshProUGUI>();

        for (int i = 65; i < 70; i++) {
            string key = "Field" + (char)i;

            TextMeshProUGUI title = GameObject.Find(key + "-title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI datum = GameObject.Find(key + "-data").GetComponent<TextMeshProUGUI>();

            if (title != null) {
                titles.Add(key, title);
                title.text = key;
            }
            if (datum != null) {
                datums.Add(key, datum);
                datum.text = "[...]";
            }
        }
        
        titles.Add("Debug", GameObject.Find("Debug-title").GetComponent<TextMeshProUGUI>());
        datums.Add("Debug", GameObject.Find("Debug-data").GetComponent<TextMeshProUGUI>());

        initialized = true;

        if (enableDebug) {
            // datums["Debug"].text = "Debug: Enabled" + "\n";
            datums["Debug"].text = "........................................................................." + "\n";
            Log("Debug: Enabled");
        }
    }

    public string GetStatus()
    {
        if (!initialized) return "Debugger is not yet initialized.";
        return "Debugger is initialized.";
    }

    // e.g. "FieldA|Title"
    public void SetFieldTitle(string value)
    {
        if (!initialized) return;
        string field = value.Split('|')[0];
        string title = value.Split('|')[1];
        titles[field].text = title;
    }

    // e.g. "FieldA|Data"
    public void SetFieldData(string value)
    {
        if (!initialized) return;
        string field = value.Split('|')[0];
        string data = value.Split('|')[1];
        datums[field].text = data;
    }

    /// <summary>
    // Logs a message. If the number of lines exceeds maxLines:
    // 1. Removes the first line
    // 2. Shifts all lines up by one
    // 3. Adds the new message to the last line
    /// </summary>
    public void Log(string message)
    {
        Debug.Log(message);
        
        if (!initialized || !enableDebug) return;

        if (datums["Debug"].text.Split('\n').Length >= maxLines) {
            ShiftLines(message);
        } else {
            datums["Debug"].text += message + "\n";
        }
    }

    /// <summary>
    /// Creates an array of strings for each line, then shifts each index "up".
    /// Intended to be only be called if the number of lines exceeds maxLines.
    /// </summary>
    private void ShiftLines(string message)
    {
        if (!initialized) return;

        string[] arr = datums["Debug"].text.Split('\n');
        for (int i = 0; i < arr.Length - 1; i++)
        {
            arr[i] = arr[i + 1];
        }
        arr[arr.Length - 1] = message;
        
        datums["Debug"].text = string.Join("\n", arr);
    }





    // private void ClearLines()
    // {
    //     if (!initialized) return;
    //     string[] arr = datums["Debug"].text.Split('\n');
    //     if (arr.Length >= maxLines)
    //     {
    //         datums["Debug"].text = "";
    //     }
    // }

    public void ClearDebug()
    {
        if (!initialized) return;
        datums["Debug"].text = "........................................................................." + "\n";
    }
}
