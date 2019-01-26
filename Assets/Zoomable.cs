using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoomable : MonoBehaviour
{
    // Start is called before the first frame update
    public float zoomIncrease = 1f;
    private float zoomLevel;
    void Start()
    {
        zoomLevel = zoomIncrease;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetZoomIncrease() {
        float f = this.zoomIncrease;
        List<Zoomable>zoomzoom = new List<Zoomable>();
        this.GetComponentsInChildren<Zoomable>(false, zoomzoom);
        foreach(Zoomable z in zoomzoom) {
            f += z.zoomIncrease;
        }
        return f;
    }
}
