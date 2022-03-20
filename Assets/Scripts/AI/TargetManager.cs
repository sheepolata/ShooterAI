using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TargetManager : MonoBehaviour
{

    public GameObject owner;

    List<bool> onEnterConditions = new List<bool>();
    List<bool> onStayConditions = new List<bool>();
    List<bool> onExitConditions = new List<bool>();

    public List<GameObject> TargetList {
        get {return targetList;}
    }
    public List<GameObject> targetList = new List<GameObject>();

    public GameObject getFirstTarget(){
        return targetList.Count > 0 ? targetList[0] : null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        onEnterConditions.Clear();
        onEnterConditions.Add(other.tag == "TargetPractice" && !targetList.Contains(other.gameObject) && !other.gameObject.GetComponent<Target>().IsDead);
        onEnterConditions.Add(other.tag == "Targetable" && !targetList.Contains(other.gameObject) && owner.GetComponent<Team>().team != other.gameObject.GetComponent<Team>().team);

        if(onEnterConditions.Any(x => x)){
            RaycastHit2D[] hits = Physics2D.RaycastAll(owner.transform.position, (other.gameObject.transform.position - owner.transform.position).normalized);
            if(other.gameObject == hits[1].collider.gameObject) targetList.Add(other.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        onStayConditions.Clear();
        onStayConditions.Add(other.tag == "TargetPractice" && other.gameObject.GetComponent<Team>().team != owner.GetComponent<Team>().team);
        onStayConditions.Add(other.tag == "Targetable" && other.gameObject.GetComponent<Team>().team != owner.GetComponent<Team>().team);
        
        if(onStayConditions.Any(x => x)){
            RaycastHit2D[] hits = Physics2D.RaycastAll(owner.transform.position, (other.gameObject.transform.position - owner.transform.position).normalized);
            if (other.gameObject.GetComponent<Target>() != null) {
                if(other.gameObject == hits[1].collider.gameObject && !targetList.Contains(other.gameObject) && !other.gameObject.GetComponent<Target>().IsDead){ 
                    targetList.Add(other.gameObject); 
                }
                
                else if((other.gameObject != hits[1].collider.gameObject && targetList.Contains(other.gameObject)) || other.gameObject.GetComponent<Target>().IsDead) { 
                    targetList.Remove(other.gameObject);
                }
            }
            else if (other.gameObject.GetComponent<Soldier>() != null) {
                if(other.gameObject == hits[1].collider.gameObject && !targetList.Contains(other.gameObject)){ 
                    targetList.Add(other.gameObject); 
                }
                
                else if((other.gameObject != hits[1].collider.gameObject && targetList.Contains(other.gameObject))) { 
                    targetList.Remove(other.gameObject);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        onExitConditions.Clear();
        onExitConditions.Add(other.tag == "TargetPractice" && targetList.Contains(other.gameObject));
        onExitConditions.Add(other.tag == "Targetable" && targetList.Contains(other.gameObject));

        if(onExitConditions.Any(x => x)) {
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
