using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public GameObject camTarget;
    public Text UICanShootCD;
    public Image ShootCDImg;

    bool shootcd_settozero = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ############# UPDATE UI #############
        if (camTarget.GetComponent<Shoot>().CanShoot) {
            UICanShootCD.text = "Shoot";
            UICanShootCD.color = Color.black;
            ShootCDImg.fillAmount = 0;
            shootcd_settozero = true;
        }
        else{
            if (shootcd_settozero){
                ShootCDImg.fillAmount = 1;
                shootcd_settozero = false;
            }

            UICanShootCD.text = string.Format("{0:f2}s", camTarget.GetComponent<Shoot>().TimerToShoot);
            UICanShootCD.color = Color.black;
            ShootCDImg.fillAmount = camTarget.GetComponent<Shoot>().TimerToShoot / camTarget.GetComponent<Shoot>().TimerToShootMax;
        }

    }
}
