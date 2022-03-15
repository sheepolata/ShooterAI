using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{

    public GameObject owner;

    public List<GameObject> TargetList {
        get {return targetList;}
    }
    public List<GameObject> targetList = new List<GameObject>();

    public GameObject getFirstTarget(){
        return targetList.Count > 0 ? targetList[0] : null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Target" && !targetList.Contains(other.gameObject) && !other.gameObject.GetComponent<Target>().IsDead){
            RaycastHit2D[] hits = Physics2D.RaycastAll(owner.transform.position, (other.gameObject.transform.position - owner.transform.position).normalized);
            // targetList.Add(other.gameObject);
            if(other.gameObject == hits[1].collider.gameObject) targetList.Add(other.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Target"){
            RaycastHit2D[] hits = Physics2D.RaycastAll(owner.transform.position, (other.gameObject.transform.position - owner.transform.position).normalized);

            if(other.gameObject == hits[1].collider.gameObject && !targetList.Contains(other.gameObject) && !other.gameObject.GetComponent<Target>().IsDead){ 
                targetList.Add(other.gameObject); 
            }
            else if((other.gameObject != hits[1].collider.gameObject && targetList.Contains(other.gameObject)) || other.gameObject.GetComponent<Target>().IsDead) { 
                targetList.Remove(other.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Target" && targetList.Contains(other.gameObject)){
            targetList.Remove(other.gameObject);
        }
    }

    void OnDrawGizmos() {
        foreach (GameObject go in targetList) {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(owner.transform.position, go.transform.position);
            Gizmos.color = Color.white;
        }
    }
}
