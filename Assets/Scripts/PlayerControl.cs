using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

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
        Rigidbody2D rb = gameObject.GetComponent<Soldier>().getRigidBody2D();
        rb.MovePosition(rb.position + movement * gameObject.GetComponent<Soldier>().moveSpeed * Time.fixedDeltaTime);

        Vector2 lookDir = mousepos - gameObject.GetComponent<Soldier>().getPosition();
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        
        rb.rotation = angle;
    }
}
