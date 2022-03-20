using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{   
    public GameObject squad;

    public float moveSpeed = 5;
    public Rigidbody2D rb;
    
    HPHandler hph;

    public bool IsDead{
        get {return dead;}
    }
    bool dead = false;

    public bool IsMoving{
        get { return moving;}
        set { moving = value;}
    }
    bool moving = false;

    public float MoveSpeedFactor {
        get {return moveSpeedFactor;}
        set {moveSpeedFactor = value/moveSpeed;}
    }
    float moveSpeedFactor = 1f;
    
    void Start() {
        hph = gameObject.GetComponent<HPHandler>();
    }

    // Update is called once per frame
    void Update() {
    }

    void FixedUpdate() {
        // GetComponent<CircleCollider2D>().enabled = GetComponent<PlayerControl>().enabled;
        // GetComponent<CircleCollider2D>().isTrigger = !GetComponent<PlayerControl>().enabled;
    }

    public Rigidbody2D getRigidBody2D() {
        return rb;
    }

    public Vector2 getPosition() {
        return rb.position;
    }

    public float getLookDir() {
        return rb.rotation;
    }

    public void onShotTaken(float dmg){
        if(hph == null || !hph.enabled || dead){
            return;
        }

        TakeDamageReturn r = hph.TakeDamage(dmg);
        switch(r) {
            case TakeDamageReturn.Dead: {
                dead = true;
                squad.GetComponent<Squad>().squadmates.Remove(this.gameObject);
                Destroy(gameObject);
                break;
            }
            case TakeDamageReturn.Alive:
            case TakeDamageReturn.Critical: {
                break;
            }
        }
    }
}
