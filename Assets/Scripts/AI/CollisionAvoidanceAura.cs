using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidanceAura : MonoBehaviour
{   
    public GameObject owner;

    List<string> avoidanceTagsDefaults = new List<string>();
    List<string> avoidanceTagsObstacles = new List<string>();

    public List<GameObject> avoidThese = new List<GameObject>();
    public List<Vector2> avoidVectors = new List<Vector2>();

    void Start() {
        avoidanceTagsDefaults.Add("TargetPractice");
        avoidanceTagsDefaults.Add("Targetable");

        avoidanceTagsObstacles.Add("Obstacle");
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if(other.gameObject.activeInHierarchy && avoidanceTagsDefaults.Contains(other.tag)){
            avoidThese.Add(other.gameObject);
        }
        else if(other.gameObject.activeInHierarchy && avoidanceTagsObstacles.Contains(other.tag)){
            // Vector2 mypos = new Vector2(owner.transform.position.x, owner.transform.position.y);
            // Vector2 otherpos = new Vector2(other.transform.position.x, other.transform.position.y);
            avoidVectors.Add(other.transform.up);
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
            if (avoidVectors.Contains(other.transform.up)) avoidVectors.Remove(other.transform.up);
        }
    }
}
