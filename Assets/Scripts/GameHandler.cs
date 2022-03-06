using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    // Camera stuff
    public Camera cam;
    public GameObject camTarget;
    public bool offsetCamOnLookDir = true;
    float zoomFactor = 1.0f;
    float zoomSpeed = 5.0f;
    private float originalSize = 0f;

    // Start is called before the first frame update
    void Start() {
        originalSize = cam.orthographicSize;
    }

    // Update is called once per frame
    void Update() {
        // ############# UPDATE CAMERA #############
        // Center camera on target
        Vector2 campos = camTarget.GetComponent<Soldier>().getPosition();
        
        // Offset camera according to look direction
        if(offsetCamOnLookDir){
            Vector2 offset = new Vector2(Mathf.Cos((camTarget.GetComponent<Soldier>().getLookDir()+90f) * Mathf.Deg2Rad), Mathf.Sin((camTarget.GetComponent<Soldier>().getLookDir()+90f) * Mathf.Deg2Rad));
            campos += offset * new Vector2(0.2f, 0.4f);
        }
        cam.transform.position =  new Vector3(campos.x, campos.y, -10);

        // Handle zoom with mouse wheel
        zoomFactor = Mathf.Clamp(zoomFactor - Input.mouseScrollDelta.y/10f, 0.5f, 3f);
        float targetSize = originalSize * zoomFactor;
        if (targetSize != cam.orthographicSize) {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
        }


    }
}
