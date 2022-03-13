using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public Camera cam;
    Vector2 movement;
    Vector2 mousepos;

    bool hasSoldierScript = false;
    bool hasShootScript = false;

    void Start(){
        hasSoldierScript = GetComponent<Soldier>() != null;
        hasShootScript = GetComponent<Shoot>() != null;
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousepos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (hasSoldierScript) {
            GetComponent<Soldier>().IsMoving = !(movement.x == 0 && movement.y == 0);
        }

        if(Input.GetKeyDown("r")){
            if (hasShootScript) {
                GetComponent<Shoot>().Reload();
            }
        }
    }

    void FixedUpdate() {
        Rigidbody2D rb = gameObject.GetComponent<Soldier>().getRigidBody2D();
        rb.MovePosition(rb.position + movement * gameObject.GetComponent<Soldier>().moveSpeed * Time.fixedDeltaTime);

        Vector2 lookDir = mousepos - gameObject.GetComponent<Soldier>().getPosition();
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        
        rb.rotation = angle;
    }
}
