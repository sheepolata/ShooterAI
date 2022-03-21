using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DelaunatorSharp;
using System.Linq;
using DelaunatorSharp.Unity.Extensions;

[ExecuteInEditMode]
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

    private List<IPoint> delaunayPoints = new List<IPoint>();
    private Delaunator delaunator;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform) {
            waypointList.Add(child);
            delaunayPoints.Add(new Point(child.position.x, child.position.y));
        }

        ComputeDelaunay();

        // StartCoroutine(UpdateDelaunay());

        isReady = true;
    }

    void Update(){
    }

    void ComputeDelaunay() {
        delaunator = null;
        delaunator = new Delaunator(delaunayPoints.ToArray());

        foreach(Transform wp in waypointList) {
            wp.gameObject.GetComponent<Waypoint>().next.Clear();
            wp.gameObject.GetComponent<Waypoint>().previous.Clear();
        }

        delaunator.ForEachTriangleEdge(edge =>
            {
                Transform p1 = GetWaypointFromVector3(edge.P.ToVector3());
                Transform p2 = GetWaypointFromVector3(edge.Q.ToVector3());

                RaycastHit2D[] hits = Physics2D.RaycastAll(p1.position, (p2.position - p1.position).normalized, Vector3.Distance(p1.position, p2.position));

                if(hits.All(x => x.collider.tag != "Obstacle")){
                    p1.gameObject.GetComponent<Waypoint>().next.Add(p2);
                    p2.gameObject.GetComponent<Waypoint>().previous.Add(p1);
                }
            });
    }

    Transform GetWaypointFromVector3(Vector3 v){
        foreach(Transform wp in waypointList) {
            if(v.x == wp.position.x && v.y == wp.position.y) return wp;
        }
        return null;
    }

    IEnumerator UpdateDelaunay() {
        while(true){
            Debug.Log("Update delaunay");
            // https://www.newcastle.edu.au/__data/assets/pdf_file/0017/22508/13_A-fast-algorithm-for-constructing-Delaunay-triangulations-in-the-plane.pdf
            yield return new WaitForSecondsRealtime(1f);
        }
    }
}
