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

    IEnumerator UpdateDelaunay() {
        // https://www.newcastle.edu.au/__data/assets/pdf_file/0017/22508/13_A-fast-algorithm-for-constructing-Delaunay-triangulations-in-the-plane.pdf
        yield return new WaitForSecondsRealtime(1f);
    }
}
