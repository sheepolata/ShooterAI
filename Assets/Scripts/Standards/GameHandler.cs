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
    public float camOffset = 2f;

    // Camera change
    public GameObject Entities;
    public GameObject Squads;
    List<GameObject> viewableEntities = new List<GameObject>();
    bool camTargetChanged = false;
    float camTransitionDistance = 0f;
    float camTransitionSpeed = 0f;

    [Min(0)]
    public float minZoom = 0.5f;
    [Min(0)]
    public float maxZoom = 3f;
    float zoomAtTransition = 0f;

    // Start is called before the first frame update
    void Start() {
        originalSize = cam.orthographicSize;
    }

    // Update is called once per frame
    void Update() {
        Soldier soldierScript = camTarget.GetComponent<Soldier>();

        // ############# UPDATE CAMERA #############
        Vector2 campos = Vector2.zero;
        if(soldierScript != null){
            // Center camera on target
            campos = soldierScript.getPosition();
            
            // Offset camera according to look direction
            if(offsetCamOnLookDir){
                Vector2 offset = new Vector2(Mathf.Cos((soldierScript.getLookDir()+90f) * Mathf.Deg2Rad), Mathf.Sin((soldierScript.getLookDir()+90f) * Mathf.Deg2Rad));
                campos += offset * new Vector2(0.1f, 0.2f) * camOffset;
            }
        }
        else{
            campos = new Vector2(camTarget.transform.position.x, camTarget.transform.position.y);
        }
        Vector3 newCamPos = new Vector3(campos.x, campos.y, -10);

        if (camTargetChanged) {

            float currentDistance = Vector3.Distance(cam.transform.position, newCamPos);
            if(currentDistance > camTransitionDistance/2f) {
                float factor = ((currentDistance/camTransitionDistance) - 0.5f) / (1.0f - 0.5f);
                camTransitionSpeed = factor * 4.5f + 0.5f;
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, maxZoom*originalSize, Time.unscaledDeltaTime * camTransitionSpeed);
            }
            else {
                float factor = ((currentDistance/camTransitionDistance) - 0.0f) / (0.5f - 0.0f);
                camTransitionSpeed = (1f-factor) * 4.5f + 0.5f;
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomAtTransition, Time.unscaledDeltaTime * camTransitionSpeed);
            }
            
            cam.transform.position = Vector3.Lerp(cam.transform.position, newCamPos, camTransitionSpeed * Time.unscaledDeltaTime);
            if(Vector3.Distance(cam.transform.position, newCamPos) < 0.05f) {
                camTargetChanged = false;
                ResumeGame();
            }
        }
        else {
            cam.transform.position = newCamPos;
        }

        // Handle zoom with mouse wheel
        zoomFactor = Mathf.Clamp(zoomFactor - Input.mouseScrollDelta.y/10f, minZoom, maxZoom);
        float targetSize = originalSize * zoomFactor;
        if (targetSize != cam.orthographicSize) {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
        }

        // Handle change of camera target
        if(Input.GetKeyDown(KeyCode.Tab) && viewableEntities.Count > 1) {
            PauseGame();
            int camTargetIndex = viewableEntities.IndexOf(camTarget);
            int newTargetIndex = (camTargetIndex + 1)%viewableEntities.Count;

            camTarget = viewableEntities[newTargetIndex];
            GetComponent<UIHandler>().camTarget = camTarget;
            camTargetChanged = true;

            Vector2 futureCamPos = Vector2.zero;
            if(soldierScript != null){
                futureCamPos = soldierScript.getPosition();
                // Offset camera according to look direction
                if(offsetCamOnLookDir){
                    Vector2 offset = new Vector2(Mathf.Cos((soldierScript.getLookDir()+90f) * Mathf.Deg2Rad), Mathf.Sin((soldierScript.getLookDir()+90f) * Mathf.Deg2Rad));
                    futureCamPos += offset * new Vector2(0.1f, 0.2f) * camOffset;
                }
            } 
            else {
                futureCamPos = new Vector2(camTarget.transform.position.x, camTarget.transform.position.y);
            }
            Vector3 futureCamPos3 = new Vector3(futureCamPos.x, futureCamPos.y, -10);
            camTransitionDistance = Vector3.Distance(cam.transform.position, futureCamPos3);

            zoomAtTransition = cam.orthographicSize;
        }
    }

    void FixedUpdate() {
        viewableEntities.Clear();

        // for(int i = 0; i < Entities.transform.childCount; i++){
        //     GameObject child = Entities.transform.GetChild(i).gameObject;
        //     if(child.activeInHierarchy && child.GetComponent<Soldier>() != null){
        //         viewableEntities.Add(child);
        //     }
        // }
        for(int i = 0; i < Squads.transform.childCount; i++){
            GameObject child = Squads.transform.GetChild(i).gameObject;
            if(child.activeInHierarchy){
                viewableEntities.Add(child);
            }
        }
    }

    public void PauseGame () {
        Time.timeScale = 0f;
    }

    public void SlowMoGame () {
        Time.timeScale = .25f;
    }

    public void ResumeGame () {
        Time.timeScale = 1f;
    }
}
