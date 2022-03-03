using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    public float dispersion = 90f;

    public float timeBetweenBullets = 0.05f;
    public int numberOfBullets = 50;
    bool canShoot = true;
    public float timeBetweenShots = 1.2f;
    float timerToShoot = 0f;

    
    void Update(){
        if(Input.GetButton("Fire1") && canShoot) {
            timerToShoot = timeBetweenShots + ((numberOfBullets-1) * timeBetweenBullets);
            StartCoroutine(TakeShot());
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
