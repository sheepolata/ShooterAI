using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{   
    public Camera cam;
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

        // txt.transform.localScale = Vector3.Max(txt.transform.localScale - (Vector3.one * retractSpeed * Time.deltaTime), Vector3.one * textScale);
        fontSizef = Mathf.Max(fontSizef - (retractSpeedFontSize * Time.deltaTime), textFontSize);
        txt.fontSize = Mathf.RoundToInt(fontSizef);
    }

    public void onShotTaken(float dmg){
        damageTaken += dmg;
        resetTimer = resetTimerMax;
        txt.text = string.Format("{0:d2}", Mathf.RoundToInt(damageTaken));

        // txt.transform.localScale = Vector3.Min(txt.transform.localScale + (Vector3.one * growthTick), new Vector3 (maxTextScale, maxTextScale, maxTextScale));
        // retractSpeed = (txt.transform.localScale.x - textScale) / resetTimerMax;
        fontSizef = Mathf.Min(fontSizef + ((float)growthTickFontSize * dmg), (float)maxTextFontSize);
        retractSpeedFontSize = (fontSizef - textFontSize) / resetTimerMax;

    }


}
