using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullet : MonoBehaviour
{
    public GameObject creator;
    public float lifespan = 10f;

    void Start(){
        Destroy(gameObject, lifespan);
    }
    
    void OnCollisionEnter2D(Collision2D collision){
        if(collision.collider.gameObject != creator && collision.collider.gameObject.tag != "Bullet"){
            Debug.Log(collision.collider.gameObject.tag);
            Destroy(gameObject);
        }
    }

}
