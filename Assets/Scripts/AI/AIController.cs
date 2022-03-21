using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AIController : MonoBehaviour
{

    GameObject squad;
    Squad squadScript;

    public GameObject WaypointManager;
    public GameObject currentWaypoint;

    public float rotationSpeed = 8.0f;

    Vector2 movement = Vector2.zero;

    public GameObject targetManagerGO;
    TargetManager targetManager;
    Shoot shootScript;
    GameObject currentTarget;
    Rigidbody2D rb;
    Soldier soldierScript;

    public GameObject avoidanceManager;
    CollisionAvoidanceAura collisionAvoidanceAura;

    float localMoveSpeed;
    float localRotationSpeed;

    GameObject previousTarget;

    void Start() {
        targetManager = targetManagerGO.GetComponent<TargetManager>();
        shootScript = GetComponent<Shoot>();
        soldierScript = gameObject.GetComponent<Soldier>();
        rb = soldierScript.getRigidBody2D();
        localMoveSpeed = soldierScript.moveSpeed;
        localRotationSpeed = rotationSpeed;

        collisionAvoidanceAura = avoidanceManager.GetComponent<CollisionAvoidanceAura>();
        squad = GetComponent<Soldier>().squad;
        squadScript = squad.GetComponent<Squad>();
    }

    // Update is called once per frame
    void Update()
    {


        updateWaypoint();
        Vector3 movementV3 = Vector3.zero;

        if(currentWaypoint != null) {
            movementV3 = currentWaypoint.transform.position - transform.position;
            movementV3.Normalize();
        }
        
        foreach(GameObject avoid in collisionAvoidanceAura.avoidThese){
            Vector3 tmp = (transform.position - avoid.transform.position);
            tmp.Normalize();
            movementV3 += tmp * 1f;
        }
        
        Vector2 newMovement = new Vector2(movementV3.x, movementV3.y); 

        foreach(Vector2 avoid in collisionAvoidanceAura.avoidVectors){
            newMovement += avoid * .5f;
        }

        newMovement.Normalize();

        movement = Vector2.Lerp(movement, newMovement, 5f * Time.deltaTime);

        soldierScript.IsMoving = !(movement.x == 0 && movement.y == 0);
        soldierScript.MoveSpeedFactor = localMoveSpeed;

        GameObject _target = pickTarget();
        if (_target != null){
            previousTarget = _target;

            localMoveSpeed = soldierScript.moveSpeed * .4f;
            localRotationSpeed = rotationSpeed * 1.8f;

            Vector3 _dir2target = _target.transform.position - transform.position;
            
            float angle = Mathf.Atan2(_dir2target.y, _dir2target.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = Mathf.LerpAngle(rb.rotation, angle, localRotationSpeed * Time.deltaTime);
            float dist2Target = Vector3.Distance(transform.position, _target.transform.position);

            if(shootScript.CanShoot && (Mathf.Abs(Mathf.DeltaAngle(angle, rb.rotation)) < (shootScript.dispersion)) && dist2Target < shootScript.maxRange) {
                shootScript.shoot();
            }
            else if (hasToReload()){
                shootScript.Reload();
            }

        }
        else if (shootScript.IsShooting && previousTarget != null){
            Vector3 _dir2target = previousTarget.transform.position - transform.position;
            
            float angle = Mathf.Atan2(_dir2target.y, _dir2target.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = Mathf.LerpAngle(rb.rotation, angle, localRotationSpeed * Time.deltaTime);
        }
        else {
            // if(!shootScript.IsMagazineFull() && !shootScript.IsReloading){
            if(shootScript.IsMagazineCritical() && !shootScript.IsReloading){
                shootScript.Reload();
            }

            localMoveSpeed = soldierScript.moveSpeed;
            localRotationSpeed = rotationSpeed;

            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = Mathf.LerpAngle(rb.rotation, angle, localRotationSpeed * Time.deltaTime);
        }
    }

    bool hasToReload() {
        return shootScript.IsMagazineCritical() && !shootScript.IsReloading;
    }

    void FixedUpdate() {
        rb.MovePosition(rb.position + movement * localMoveSpeed * Time.fixedDeltaTime);
    }

    void updateWaypoint() {
        currentWaypoint = squadScript.currentWaypoint;
        float dist = Vector2.Distance(currentWaypoint.transform.position, transform.position);
        if (dist < squadScript.waypointValidationThreshold){
            currentWaypoint = null;
        }
    }

    // void chooseNextWaypoint(){
    //     if ( Vector3.Distance(transform.position, currentWaypoint.transform.position) < 0.3f ){
    //         currentWaypoint = currentWaypoint.GetComponent<Waypoint>().NextWaypointRandom().gameObject;
    //     }
    // }

    GameObject pickTarget(){
        return targetManager.getFirstTarget();
    }

    void OnDrawGizmos() {
        if(Application.isPlaying){
            Gizmos.color = Color.green;
            Gizmos.DrawLine(rb.position, rb.position + movement);
            Gizmos.color = Color.white;
        }
    }
}
