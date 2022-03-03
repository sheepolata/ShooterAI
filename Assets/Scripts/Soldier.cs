using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public string MOVEMENT = "##################";
    public float moveSpeed = 5;
    public Rigidbody2D rb;
    public Camera cam;
    Vector2 movement;
    Vector2 mousepos;

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousepos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate() {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        Vector2 lookDir = mousepos - getPosition();
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        if(angle < 0){angle += 360;}
        
        rb.rotation = angle;
    }

    public Vector2 getPosition(){
        return rb.position;
    }

    public float getLookDir(){
        return rb.rotation;
    }
}
