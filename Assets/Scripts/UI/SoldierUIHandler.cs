using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierUIHandler : MonoBehaviour
{
    public GameObject owner;
    public Image clipGauge;
    public Image healthBar; 

    Gradient clipGaugeGradient;
    GradientColorKey[] clipGaugeColorKey;
    GradientAlphaKey[] clipGaugeAlphaKey;

    Gradient healthBarGradient;
    GradientColorKey[] healthBarColorKey;
    GradientAlphaKey[] healthBarAlphaKey;

    Shoot shootScript;
    HPHandler hph;

    void Start() {
        shootScript = owner.GetComponent<Shoot>();
        hph = owner.GetComponent<HPHandler>();

        clipGaugeGradient = new Gradient();

        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        clipGaugeColorKey = new GradientColorKey[3];
        clipGaugeColorKey[0].color = Color.red;
        clipGaugeColorKey[0].time = 0.0f;
        clipGaugeColorKey[1].color = Color.yellow;
        clipGaugeColorKey[1].time = 0.5f;
        clipGaugeColorKey[2].color = Color.cyan;
        clipGaugeColorKey[2].time = 1.0f;

        // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        clipGaugeAlphaKey = new GradientAlphaKey[2];
        clipGaugeAlphaKey[0].alpha = 1.0f;
        clipGaugeAlphaKey[0].time = 0.0f;
        clipGaugeAlphaKey[1].alpha = 1.0f;
        clipGaugeAlphaKey[1].time = 1.0f;

        clipGaugeGradient.SetKeys(clipGaugeColorKey, clipGaugeAlphaKey);

        healthBarGradient = new Gradient();

        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        healthBarColorKey = new GradientColorKey[3];
        healthBarColorKey[0].color = Color.red;
        healthBarColorKey[0].time = 0.0f;
        healthBarColorKey[1].color = Color.yellow;
        healthBarColorKey[1].time = 0.4f;
        healthBarColorKey[2].color = Color.green;
        healthBarColorKey[2].time = 1.0f;

        // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        healthBarAlphaKey = new GradientAlphaKey[2];
        healthBarAlphaKey[0].alpha = 1.0f;
        healthBarAlphaKey[0].time = 0.0f;
        healthBarAlphaKey[1].alpha = 1.0f;
        healthBarAlphaKey[1].time = 1.0f;

        healthBarGradient.SetKeys(healthBarColorKey, healthBarAlphaKey);
    }

    // Update is called once per frame
    void Update()
    {
        // transform.rotation = Quaternion.identity;
        // GetComponent<RectTransform>().localRotation = Quaternion.identity;

        if (!shootScript.IsReloading) {
            clipGauge.fillAmount = (float)shootScript.CurrentClipSize / (float)shootScript.maxClipSize;
            clipGauge.color = clipGaugeGradient.Evaluate(clipGauge.fillAmount);
        }
        else{
            clipGauge.color = Color.red;
            clipGauge.fillAmount = 1 - (shootScript.TimerToReload / shootScript.reloadTime);
        }

        healthBar.fillAmount = hph.CurrentHP / hph.maxHP;
        healthBar.color = healthBarGradient.Evaluate(healthBar.fillAmount);
    }
}
