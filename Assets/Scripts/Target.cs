using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{   
    public Camera cam;
    float damageTaken = 0;

    Text txt;
    float resetTimerMax = 2f;
    float resetTimer = 2f;

    void Start(){
        transform.GetChild(0).GetComponent<Canvas>().worldCamera = cam;
        txt = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        txt.text = "0";

    }

    void Update(){
        resetTimer -= Time.deltaTime;
        if (resetTimer <= 0){
            damageTaken = 0;
            txt.text = string.Format("{0:d2}", Mathf.RoundToInt(damageTaken));
            resetTimer = 0;
        }
    }

    public void onShotTaken(float dmg){
        damageTaken += dmg;
        resetTimer = resetTimerMax;
        txt.text = string.Format("{0:d2}", Mathf.RoundToInt(damageTaken));
    }


}
