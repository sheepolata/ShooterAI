using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class impact : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Animator>().Play("ph_impact");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnAnimationEnd(){
        Destroy(gameObject);
    }
}
