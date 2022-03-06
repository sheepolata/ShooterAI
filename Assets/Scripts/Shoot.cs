using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject impactVFX;
    public GameObject nozzleflashVFX;
    public float bulletForce = 20f;
    public float dispersion = 90f;

    public float timeBetweenBullets = 0.05f;
    public int numberOfBullets = 50;
    public bool CanShoot{
        get { return canShoot;}
    }
    bool canShoot = true;
    public float timeBetweenShots = 1.2f;
    public float TimerToShoot{
        get { return timerToShoot;}
    }
    float timerToShoot = 0f;

    public float TimerToShootMax{
        get { return timeBetweenShots + ((numberOfBullets-1) * timeBetweenBullets);}
    }


    
    void Update(){
        if(Input.GetButton("Fire1") && canShoot && (GetComponent<PlayerControl>() != null && GetComponent<PlayerControl>().enabled)) {
            timerToShoot = TimerToShootMax;
            // StartCoroutine(TakeShot());
            StartCoroutine(TakeShotRayCast());
            canShoot = false;
        }
    }

    void FixedUpdate()
    {
        if(!canShoot){
            timerToShoot -= Time.fixedDeltaTime;
            if(timerToShoot <= 0) {
                canShoot = true;
            }
        }
    }

    IEnumerator TakeShotRayCast(){
        OneShotRayCast();
        for(int i = 0; i < numberOfBullets-1; i++){
            yield return new WaitForSeconds(timeBetweenBullets);
            OneShotRayCast();
        }
    }


    void OneShotRayCast() {
        GameObject _nozzleflashVFX = Instantiate(nozzleflashVFX, firePoint.position, firePoint.rotation);
        _nozzleflashVFX.transform.Rotate(0, 0, Random.Range(-dispersion, dispersion));

        RaycastHit2D hit = Physics2D.Raycast(_nozzleflashVFX.transform.position, _nozzleflashVFX.transform.up);

        GameObject _impactVFX = Instantiate(impactVFX, hit.point, new Quaternion(0, 0, 0, 0));

        Destroy(_nozzleflashVFX, 0.05f);
    }


    IEnumerator TakeShot(){
        OneShot();
        for(int i = 0; i < numberOfBullets-1; i++){
            yield return new WaitForSeconds(timeBetweenBullets);
            OneShot();
        }
    }

    void OneShot() {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        
        bullet.GetComponent<BasicBullet>().creator = gameObject;

        Debug.Log(dispersion);
        bullet.transform.Rotate(0, 0, Random.Range(-dispersion, dispersion));

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(bullet.transform.up * bulletForce, ForceMode2D.Impulse);
    }
}
