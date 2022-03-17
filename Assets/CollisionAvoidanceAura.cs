using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidanceAura : MonoBehaviour
{

    List<string> avoidanceTagsDefaults = new List<string>();
    List<string> avoidanceTagsObstacles = new List<string>();

    public List<GameObject> avoidThese = new List<GameObject>();

    void Start() {
        avoidanceTagsDefaults.Add("TargetPractice");
        avoidanceTagsDefaults.Add("Targetable");
        avoidanceTagsObstacles.Add("Obstacle");
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if(avoidanceTagsDefaults.Contains(other.tag)){
            avoidThese.Add(other.gameObject);
        }
        else if(avoidanceTagsObstacles.Contains(other.tag)){
            
        }
    }

    // private void OnTriggerStay2D(Collider2D other) {
    //     if(avoidanceTagsDefaults.Contains(other.tag)){
            
    //     }
    // }

    private void OnTriggerExit2D(Collider2D other) {
        if(avoidanceTagsDefaults.Contains(other.tag)){
            if (avoidThese.Contains(other.gameObject)) avoidThese.Remove(other.gameObject);
        }
        else if(avoidanceTagsObstacles.Contains(other.tag)){

        }
    }
}
