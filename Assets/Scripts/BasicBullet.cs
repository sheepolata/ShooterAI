using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullet : MonoBehaviour
{
    public GameObject creator;
    
    void OnCollisionEnter2D(Collision2D collision){
        if(collision.collider.gameObject != creator){
            Destroy(gameObject);
        }
    }

}
