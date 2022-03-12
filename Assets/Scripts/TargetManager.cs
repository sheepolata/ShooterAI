using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{

    public GameObject owner;

    public List<GameObject> TargetList {
        get {return targetList;}
    }
    List<GameObject> targetList = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Target"){
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Target"){
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Target"){
        }
    }
}
