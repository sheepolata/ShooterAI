using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{

    public List<Transform> WaypointList {
        get {return waypointList;}
    }
    List<Transform> waypointList = new List<Transform>();

    public bool IsReady {
        get {return isReady;}
    }
    bool isReady = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform) {
            waypointList.Add(child);
        }
        isReady = true;
    }
}
