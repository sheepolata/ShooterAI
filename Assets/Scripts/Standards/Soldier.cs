using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{

    public float moveSpeed = 5;
    public Rigidbody2D rb;

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
}
