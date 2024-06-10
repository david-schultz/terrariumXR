using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlanets : MonoBehaviour
{
    private GameObject planet;
    private bool selected = true;
    
    void Start()
    {
        planet = this.transform.gameObject;
    }


    void Update()
    {
        if (selected)
        {
            float distanceMultiplier = 0.1f;
            if (OVRInput.GetDown(OVRInput.Button.One))
            {
                planet.transform.Translate(Vector3.up * distanceMultiplier);
            }
            if (OVRInput.GetDown(OVRInput.Button.Two))
            {
                planet.transform.Translate(Vector3.down * distanceMultiplier);
            }
        }

    }
}
