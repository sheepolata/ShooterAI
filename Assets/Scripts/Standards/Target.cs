using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{   
    public Camera cam;
    HPHandler hph;
    float damageTaken = 0;

    Text txt;
    public float resetTimerMax = 2f;
    float resetTimer = 2f;

    // float textScale = 0.01f;
    // float maxTextScale = 0.07f;
    // float retractSpeed = 0.01f;
    // float growthTick = 0.005f;

    int textFontSize = 14;
    int maxTextFontSize = 65;
    float retractSpeedFontSize;
    int growthTickFontSize = 4;
    float fontSizef = 14f;

    public bool IsDead{
        get {return dead;}
    }
    bool dead = false;
    public float deadTimer = 10f;
    float currentDeadTimer = 0f;

    void Start(){
        transform.GetChild(0).GetComponent<Canvas>().worldCamera = cam;
        txt = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        txt.text = "0";
        hph = GetComponent<HPHandler>();

        if(hph != null && hph.enabled) txt.text = string.Format("{0:d2}", Mathf.RoundToInt(hph.CurrentHP));
    }


    void Update(){

        if (IsDead) {
            currentDeadTimer -= Time.deltaTime;
            if (currentDeadTimer <= 0f){
                currentDeadTimer = 0f;
                hph.Reset();
                GetComponent<SpriteRenderer>().color = Color.red;
                dead = false;
            }
        }

        if(hph == null || !hph.enabled){
            resetTimer -= Time.deltaTime;
            if (resetTimer <= 0) {
                damageTaken = 0;
                txt.text = string.Format("{0:d2}", Mathf.RoundToInt(damageTaken));
                resetTimer = 0;
            }

            // txt.transform.localScale = Vector3.Max(txt.transform.localScale - (Vector3.one * retractSpeed * Time.deltaTime), Vector3.one * textScale);
            fontSizef = Mathf.Max(fontSizef - (retractSpeedFontSize * Time.deltaTime), textFontSize);
            txt.fontSize = Mathf.RoundToInt(fontSizef);
        }
        else {
            fontSizef = textFontSize + (hph.CurrentRegenTimer / hph.regenTimer)*(maxTextFontSize-textFontSize);
            txt.fontSize = Mathf.RoundToInt(fontSizef);
        }
    }

    void FixedUpdate(){
        if(hph != null && hph.enabled) txt.text = string.Format("{0:d2}", Mathf.RoundToInt(hph.CurrentHP));
    }

    public void onShotTaken(float dmg){
        if(dead) return;

        if(hph == null || !hph.enabled){
            damageTaken += dmg;
            resetTimer = resetTimerMax;
            txt.text = string.Format("{0:d2}", Mathf.RoundToInt(damageTaken));
            fontSizef = Mathf.Min(fontSizef + ((float)growthTickFontSize * dmg), (float)maxTextFontSize);
            retractSpeedFontSize = (fontSizef - textFontSize) / resetTimerMax;
        }
        else {
            TakeDamageReturn r = hph.TakeDamage(dmg);
            switch(r) {
                case TakeDamageReturn.Dead: {
                    dead = true;
                    hph.ForcedStopedRegen = true;
                    GetComponent<SpriteRenderer>().color = Color.gray;
                    currentDeadTimer = deadTimer;
                    break;
                }
                case TakeDamageReturn.Alive:
                case TakeDamageReturn.Critical: {
                    break;
                }
            }
            txt.text = string.Format("{0:d2}", Mathf.RoundToInt(hph.CurrentHP));
        }
    }


}
