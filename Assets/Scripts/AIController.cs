using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AIController : MonoBehaviour
{

    public GameObject WaypointManager;
    public GameObject currentWaypoint;

    public float rotationSpeed = 0.0f;

    Vector2 movement;

    GameObject currentTarget;

    // Update is called once per frame
    void Update()
    {
        chooseNextWaypoint();

        Vector3 movementV3 = currentWaypoint.transform.position - transform.position;
        movement = new Vector2(movementV3.x, movementV3.y); movement.Normalize();
    }

    void FixedUpdate() {
        Rigidbody2D rb = gameObject.GetComponent<Soldier>().getRigidBody2D();
        rb.MovePosition(rb.position + movement * gameObject.GetComponent<Soldier>().moveSpeed * Time.fixedDeltaTime);

        float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg - 90f;
        // Debug.Log(rb.rotation);
        // Debug.Log(Mathf.LerpAngle(rb.rotation, angle, rotationSpeed * Time.fixedDeltaTime));
        rb.rotation = Mathf.LerpAngle(rb.rotation, angle, rotationSpeed * Time.fixedDeltaTime);
    }

    void chooseNextWaypoint(){
        if ( Vector3.Distance(transform.position, currentWaypoint.transform.position) < 0.5f ){
            currentWaypoint = currentWaypoint.GetComponent<Waypoint>().NextWaypointRandom().gameObject;
        }
    }

    void pickTarget(){

    }
}
