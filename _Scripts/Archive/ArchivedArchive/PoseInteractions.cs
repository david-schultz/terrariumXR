using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseInteractions : MonoBehaviour
{
    private GameObject planet;
    private Vector3 scaleChange = new Vector3(0.08f, 0.08f, 0.08f);
    
    void Start()
    {
        planet = this.transform.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            IncreaseScale();
        }
        if (OVRInput.GetDown(OVRInput.RawButton.B))
        {
            DecreaseScale();
        }
    }

    public void IncreaseScale()
    {
        planet.transform.localScale += scaleChange;
    }

    public void DecreaseScale()
    {
        planet.transform.localScale -= scaleChange;
    }
}
