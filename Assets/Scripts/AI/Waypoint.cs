using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Waypoint : MonoBehaviour
{

    public List<Transform> next = new List<Transform>();

    public List<Transform> Previous {
        get {return previous;}
    }
    public List<Transform> previous = new List<Transform>();

    void Start() {
        foreach (Transform t in next) {
            t.gameObject.GetComponent<Waypoint>().Previous.Add(this.transform);
        }
    }

    void OnDrawGizmos() {

        foreach (Transform t in next) {
            // DrawArrow.ForGizmo(transform.position, (t.position - transform.position), Color.white, false, 2f, 20f);
            Gizmos.DrawLine(transform.position, t.position);
        }
    }

    public Transform NextWaypoint(int i = 0) {
        // // Check where we are
        // int thisIndex = transform.GetSiblingIndex();

        // // We have a few cases to rule out
        // if (transform.parent == null || transform.parent.childCount == 1)
        //     return null;
        // if (transform.parent.childCount <= thisIndex + 1)
        //     return transform.parent.GetChild (0);

        // // Then return whatever was next, now that we're sure it's there
        // return transform.parent.GetChild (thisIndex + 1);
        if (next.Count == 0 || i >= next.Count) return null;
        return next[i];
    }

    public Transform NextWaypointRandom() {
        return Random.value < 0.1f ? previous[Random.Range(0, previous.Count)] : next[Random.Range(0, next.Count)];
        // return next[Random.Range(0, next.Count)];
    }
}
