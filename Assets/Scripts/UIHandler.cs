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
        if(camTarget.GetComponent<Shoot>() != null) {
            Shoot shootScript = camTarget.GetComponent<Shoot>();
            if (shootScript.CanShoot) {
                ShootCDTxt.text = "Shoot";
                ShootCD.fillAmount = 0;
                shootcd_settozero = true;
            }
            else{
                if (shootcd_settozero){
                    ShootCD.fillAmount = 1;
                    shootcd_settozero = false;
                }

                ShootCDTxt.text = string.Format("{0:f2}s", shootScript.TimerToShoot);
                ShootCD.fillAmount = shootScript.TimerToShoot / shootScript.TimerToShootMax;
            }

            if (shootScript.CanShootReload) {
                ReloadTxt.text = string.Format("{0:d3}/{1:d3}", shootScript.CurrentClipSize, shootScript.maxClipSize);
                ReloadCD.fillAmount = 0;
                reload_settozero = true;
                if(shootScript.CurrentClipSize <= 0){
                    ReloadCD.fillAmount = 1;
                }
            }
            else{
                if (reload_settozero){
                    ReloadCD.fillAmount = 1;
                    reload_settozero = false;
                }
                ReloadTxt.text = string.Format("Reloading");
                ReloadCD.fillAmount = shootScript.TimerToReload / shootScript.reloadTime;
            }
        }

    }
}
