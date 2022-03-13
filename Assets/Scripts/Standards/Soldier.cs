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
    

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate() {
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
