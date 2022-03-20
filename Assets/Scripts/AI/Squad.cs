using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class Squad : MonoBehaviour
{

    public List<GameObject> squadmates = new List<GameObject>();
    [Min(0)]
    public float waypointValidationThreshold = .3f;
    
    public GameObject WaypointManager;
    public GameObject currentWaypoint;

    // Start is called before the first frame update
    // void Start()
    // {

    // }

    void Update() {
        if(squadmates.Count > 0) {  
            transform.position = new Vector3(
                                    squadmates.Average(x=>x.transform.position.x),
                                    squadmates.Average(x=>x.transform.position.y),
                                    squadmates.Average(x=>x.transform.position.z));
        }
    }

    void FixedUpdate() {
        if(squadmates.Count > 0) {
            float[] ldist = new float[squadmates.Count];
            for(int i = 0; i < squadmates.Count; ++i) {
                ldist[i] = Vector3.Distance(squadmates[i].transform.position, currentWaypoint.transform.position);
            }

            if(ldist.All(x => x <= waypointValidationThreshold*2f)){
                chooseNextWaypoint();
            }
        }
    }

    void chooseNextWaypoint(){
        // if ( Vector3.Distance(transform.position, currentWaypoint.transform.position) < 0.3f ){
        currentWaypoint = currentWaypoint.GetComponent<Waypoint>().NextWaypointRandom().gameObject;
        // }
    }
}
