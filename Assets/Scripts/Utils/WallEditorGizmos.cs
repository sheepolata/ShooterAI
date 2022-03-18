using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallEditorGizmos : MonoBehaviour
{
    void OnDrawGizmos() {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, transform.position + transform.up*.5f);
        Gizmos.color = Color.white;
    }
}
