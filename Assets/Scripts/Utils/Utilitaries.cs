using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Utilitaries
{
    public static Transform GetChildByName(Transform parent, string name){
        return parent.Find(name);
    }
}
