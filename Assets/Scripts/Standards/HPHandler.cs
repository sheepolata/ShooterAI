using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TakeDamageReturn
{
    Alive,
    Critical,
    Dead
}

public class HPHandler : MonoBehaviour
{
    [Min(0)]
    public float maxHP = 10f;
    [Min(0)]
    public float regenHP = 0.1f;
    [Min(0)]
    public float regenTimer = 2.0f;

    public float CurrentRegenTimer{
        get {return currentRegenTimer;}
    }
    float currentRegenTimer = 0f;
    
    public float CurrentHP{
        get {return currentHP;}
    }
    float currentHP = 1f;
    
    bool canRegen = true;
    public bool ForcedStopedRegen{
        get {return forcedStopedRegen;}
        set {forcedStopedRegen = value;}
    }
    bool forcedStopedRegen = false;
    
    [Range(0f,1f)]
    public float criticalTreshold = 0.2f;

    // Start is called before the first frame update
    void Start() {
        currentHP = maxHP;        
    }

    void Update() {
        if(currentRegenTimer >= 0f){
            currentRegenTimer -= Time.deltaTime;
            if(currentRegenTimer <= 0f){
                currentRegenTimer = 0f;
                canRegen = true;
            }
        }
    }

    void FixedUpdate() {
        if(canRegen && !forcedStopedRegen) currentHP = Mathf.Min(maxHP, currentHP + (regenHP * Time.fixedDeltaTime));
    }

    public void Reset(){
        currentHP = maxHP;
        canRegen = true;
        currentRegenTimer = 0f;
        forcedStopedRegen = false;
    }

    public TakeDamageReturn TakeDamage(float dmg) {
        currentHP = Mathf.Max(0, currentHP - dmg);
        canRegen = false;
        currentRegenTimer = regenTimer;

        if(currentHP <= 0){ return TakeDamageReturn.Dead;}
        else if(currentHP/maxHP <= criticalTreshold) {return TakeDamageReturn.Critical;}
        else {return TakeDamageReturn.Alive;}
    }
}
