using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public GameObject camTarget;
    public Text ShootCDTxt;
    public Image ShootCD;
    public Text ReloadTxt;
    public Image ReloadCD;

    bool shootcd_settozero = false;
    bool reload_settozero = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ############# UPDATE UI #############
        if (camTarget.GetComponent<Shoot>().CanShoot) {
            ShootCDTxt.text = "Shoot";
            ShootCD.fillAmount = 0;
            shootcd_settozero = true;
        }
        else{
            if (shootcd_settozero){
                ShootCD.fillAmount = 1;
                shootcd_settozero = false;
            }

            ShootCDTxt.text = string.Format("{0:f2}s", camTarget.GetComponent<Shoot>().TimerToShoot);
            ShootCD.fillAmount = camTarget.GetComponent<Shoot>().TimerToShoot / camTarget.GetComponent<Shoot>().TimerToShootMax;
        }
    
        if (camTarget.GetComponent<Shoot>().CanShootReload) {
            ReloadTxt.text = string.Format("{0:d3}/{1:d3}", camTarget.GetComponent<Shoot>().CurrentClipSize, camTarget.GetComponent<Shoot>().maxClipSize);
            ReloadCD.fillAmount = 0;
            reload_settozero = true;
        }
        else{
            if (reload_settozero){
                ReloadCD.fillAmount = 1;
                reload_settozero = false;
            }
            ReloadTxt.text = string.Format("Reloading");
            ReloadCD.fillAmount = camTarget.GetComponent<Shoot>().TimerToReload / camTarget.GetComponent<Shoot>().reloadTime;
        }

    }
}
