using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject impactVFX;
    public GameObject nozzleflashVFX;
    public GameObject rayVFX;
    public float bulletForce = 20f;
    public float bulletDamage = 1f;
    public float dispersion = 5f;

    public float timeBetweenBullets = 0.05f;
    public int numberOfBullets = 7;
    public float timeBetweenShots = 0.5f;
    public int maxClipSize = 28;
    public float reloadTime = 1.8f;

    public float maxRange = 6f;
    public float rangeDispersion = 1f;

    public float TimerToReload{
        get { return timerToReload;}
    }
    float timerToReload = 0f;

    public int CurrentClipSize{
        get {return currentClipSize;}
    }
    int currentClipSize;

    public bool CanShoot{
        get { return canShoot && !isReloading;}
    }
    public bool IsShooting{
        get { return !canShoot;}
    }
    bool canShoot = true;

    public bool IsReloading{
        get { return isReloading;}
    }
    bool isReloading = false;

    public float TimerToShoot{
        get { return timerToShoot;}
    }
    float timerToShoot = 0f;

    public float TimerToShootMax{
        get { return timeBetweenShots + ((numberOfBullets-1) * timeBetweenBullets);}
    }

    void Start(){
        currentClipSize = maxClipSize;
    }

    void Update(){
        if(Input.GetButton("Fire1") && CanShoot && (GetComponent<PlayerControl>() != null && GetComponent<PlayerControl>().enabled)) {
            // timerToShoot = TimerToShootMax;
            // StartCoroutine(TakeShot());
            // canShoot = false;
            shoot();
        }

        if(!canShoot){
            timerToShoot -= Time.deltaTime;
            if(timerToShoot <= 0) {
                canShoot = true;
            }
        }

        if(isReloading){
            timerToReload -= Time.deltaTime;
            if (timerToReload <= 0){
                isReloading = false;
                currentClipSize = maxClipSize;
            }
        }
    }

    public void shoot(){
        timerToShoot = TimerToShootMax;
        canShoot = false;
        StartCoroutine(TakeShotRayCast());
    }

    public void Reload(){
        isReloading = true;
        timerToReload = reloadTime;
    }

    public bool IsMagazineEmpty(){
        return CurrentClipSize <= 0;
    }

    public bool IsMagazineCritical(){
        return CurrentClipSize <= maxClipSize * 0.2f;
    }

    public bool IsMagazineFull(){
        return CurrentClipSize >= maxClipSize;
    }

    IEnumerator TakeShotRayCast(){
        int res = OneShotRayCast();
        if(res <= 0) {yield break;} 
        for(int i = 0; i < numberOfBullets-1; i++){
            yield return new WaitForSeconds(timeBetweenBullets);
            res = OneShotRayCast();
            if(res <= 0) {yield break;}
        }
    }


    int OneShotRayCast() {
        if (currentClipSize <= 0){
            return -1;
        }

        GameObject _nozzleflashVFX = Instantiate(nozzleflashVFX, firePoint.position, firePoint.rotation);
        
        // float _dispersion = dispersion * (GetComponent<Soldier>().IsMoving ? 1f : 0.25f);
        float _dispersion = dispersion * ( (GetComponent<Soldier>().MoveSpeedFactor * .75f) + .25f );
        
        _nozzleflashVFX.transform.Rotate(0, 0, Random.Range(-_dispersion, _dispersion));

        // float _range_dispersion = rangeDispersion * (GetComponent<Soldier>().IsMoving ? 1f : 0.25f);
        float _range_dispersion = rangeDispersion * ( (GetComponent<Soldier>().MoveSpeedFactor * .75f) + .25f );
        float _range = maxRange + Random.Range(-_range_dispersion, 0);
        
        RaycastHit2D hit = Physics2D.Raycast(_nozzleflashVFX.transform.position, _nozzleflashVFX.transform.up, _range);
        Vector3 scale = new Vector3(0.02f,0.02f,0.02f);
        if (hit.collider != null){
            GameObject _impactVFX = Instantiate(impactVFX, hit.point, new Quaternion(0, 0, 0, 0));
            scale.y = Vector3.Distance(hit.point, firePoint.position);
        }
        else {
            GameObject _impactVFX = Instantiate(impactVFX, firePoint.position + new Vector3(_range * _nozzleflashVFX.transform.up.x, _range * _nozzleflashVFX.transform.up.y, 0) , new Quaternion(0, 0, 0, 0));
            scale.y = _range;
        }
        GameObject _rayVFX = Instantiate(rayVFX, firePoint.position, _nozzleflashVFX.transform.rotation);
        _rayVFX.transform.localScale = scale;

        Destroy(_rayVFX, 0.05f);
        Destroy(_nozzleflashVFX, 0.05f);

        currentClipSize -= 1;

        if (hit.collider != null){
            if (hit.collider.GetComponent<Target>() != null) {hit.collider.GetComponent<Target>().onShotTaken(bulletDamage);}
            if (hit.collider.GetComponent<Soldier>() != null) {hit.collider.GetComponent<Soldier>().onShotTaken(bulletDamage);}
        }

        return 1;
    }


    // IEnumerator TakeShot(){
    //     OneShot();
    //     for(int i = 0; i < numberOfBullets-1; i++){
    //         yield return new WaitForSeconds(timeBetweenBullets);
    //         OneShot();
    //     }
    // }

    // void OneShot() {
    //     GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        
    //     bullet.GetComponent<BasicBullet>().creator = gameObject;

    //     Debug.Log(dispersion);
    //     bullet.transform.Rotate(0, 0, Random.Range(-dispersion, dispersion));

    //     Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
    //     rb.AddForce(bullet.transform.up * bulletForce, ForceMode2D.Impulse);
    // }
}
