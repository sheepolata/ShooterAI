using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Linq;

public enum CamFollowOptions {
    SOLDIER,
    SQUAD,
    FREE
}

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

    bool slowed = false;
    bool paused = false;
    bool speedup = false;

    CamFollowOptions camTargetType = CamFollowOptions.SQUAD;
    public float freeCameraSpeed = 8f;

    // Start is called before the first frame update
    void Start() {
        originalSize = cam.orthographicSize;
    }

    // Update is called once per frame
    void Update() {
        Soldier soldierScript = null;

        if(camTarget == null && camTargetType != CamFollowOptions.FREE) {
            UpdateViewableEntities();
            camTarget = viewableEntities[0];
            ChangeCamTarget(true);
        }

        if(camTarget != null) {
            // ############# UPDATE CAMERA #############
            Vector2 campos = Vector2.zero;
            if(camTargetType == CamFollowOptions.SOLDIER){
                soldierScript = camTarget.GetComponent<Soldier>();
                // Center camera on target
                campos = soldierScript.getPosition();
                
                // Offset camera according to look direction
                if(offsetCamOnLookDir){
                    Vector2 offset = new Vector2(Mathf.Cos((soldierScript.getLookDir()+90f) * Mathf.Deg2Rad), Mathf.Sin((soldierScript.getLookDir()+90f) * Mathf.Deg2Rad));
                    campos += offset * new Vector2(0.1f, 0.2f) * camOffset;
                }
            }
            else if (camTargetType == CamFollowOptions.SQUAD){
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
        }
        else if(camTargetType == CamFollowOptions.FREE) {
            Vector2 freeCamMovement = Vector2.zero;
            freeCamMovement.x = Input.GetAxisRaw("Horizontal");
            freeCamMovement.y = Input.GetAxisRaw("Vertical");
            Debug.Log(freeCamMovement);
            cam.transform.position = new Vector3(
                                        cam.transform.position.x + (freeCamMovement.x * freeCameraSpeed * Time.unscaledDeltaTime),
                                        cam.transform.position.y + (freeCamMovement.y * freeCameraSpeed * Time.unscaledDeltaTime),
                                        cam.transform.position.z
                                    );
        }

        // Handle zoom with mouse wheel
        zoomFactor = Mathf.Clamp(zoomFactor - Input.mouseScrollDelta.y/10f, minZoom, maxZoom);
        float targetSize = originalSize * zoomFactor;
        if (targetSize != cam.orthographicSize) {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.unscaledDeltaTime * zoomSpeed);
        }

        // Handle change of camera target
        if(Input.GetKeyDown(KeyCode.Tab) && camTargetType != CamFollowOptions.FREE && viewableEntities.Count > 1) {
            ChangeCamTarget();
        }
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            PauseGame();
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)) {
            SlowMoGame();
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)) {
            NormalSpeedGame();
        }
        if(Input.GetKeyDown(KeyCode.Alpha4)) {
            SpeedUpGame();
        }
        if(Input.GetKeyDown(KeyCode.Space)) {
            if(!paused) PauseGame();
            else ResumeGame();
        }

        if(Input.GetKeyDown(KeyCode.F1)){
            camTargetType = (CamFollowOptions)((int)(camTargetType+1)%((int)CamFollowOptions.GetValues(typeof(CamFollowOptions)).Cast<CamFollowOptions>().Max()+1));
            ChangeCamTarget(true, true);
        }
    }

    private void ChangeCamTarget(bool forcedTarget = false, bool toNull = false) {
            if(forcedTarget && toNull){
                camTarget = null;
                GetComponent<UIHandler>().camTarget = null;
                return;
            }

            PauseGame();
            int camTargetIndex = viewableEntities.IndexOf(camTarget);
            int newTargetIndex = (camTargetIndex + 1)%viewableEntities.Count;

            if(!forcedTarget) camTarget = viewableEntities[newTargetIndex];
            GetComponent<UIHandler>().camTarget = camTarget;
            camTargetChanged = true;

            Vector2 futureCamPos = Vector2.zero;
            if(camTargetType == CamFollowOptions.SOLDIER){
                Soldier ss = camTarget.GetComponent<Soldier>();
                futureCamPos = ss.getPosition();
                // Offset camera according to look direction
                if(offsetCamOnLookDir){
                    Vector2 offset = new Vector2(Mathf.Cos((ss.getLookDir()+90f) * Mathf.Deg2Rad), Mathf.Sin((ss.getLookDir()+90f) * Mathf.Deg2Rad));
                    futureCamPos += offset * new Vector2(0.1f, 0.2f) * camOffset;
                }
            } 
            else if (camTargetType == CamFollowOptions.SQUAD){
                futureCamPos = new Vector2(camTarget.transform.position.x, camTarget.transform.position.y);
            }
            Vector3 futureCamPos3 = new Vector3(futureCamPos.x, futureCamPos.y, -10);
            camTransitionDistance = Vector3.Distance(cam.transform.position, futureCamPos3);

            zoomAtTransition = cam.orthographicSize;
    }

    void FixedUpdate() {
        UpdateViewableEntities();
    }

    void UpdateViewableEntities(){
        switch(camTargetType){
            case CamFollowOptions.SQUAD: {
                viewableEntities.Clear();
                for(int i = 0; i < Squads.transform.childCount; i++){
                    GameObject child = Squads.transform.GetChild(i).gameObject;
                    if(child.activeInHierarchy){
                        viewableEntities.Add(child);
                    }
                }
                break;
            }
            case CamFollowOptions.SOLDIER: {
                viewableEntities.Clear();
                for(int i = 0; i < Entities.transform.childCount; i++){
                    GameObject child = Entities.transform.GetChild(i).gameObject;
                    if(child.activeInHierarchy && child.GetComponent<Soldier>() != null){
                        viewableEntities.Add(child);
                    }
                }
                break;
            }
            case CamFollowOptions.FREE: {
                viewableEntities.Clear();
                break;
            }
        }
    }

    public void PauseGame () {
        Time.timeScale = 0f;
        paused = true;
    }

    public void SlowMoGame () {
        Time.timeScale = .5f;
        slowed = true;
        speedup = false;
        paused = false;
    }

    public void NormalSpeedGame() {
        Time.timeScale = 1f;
        slowed = false;
        speedup = false;
        paused = false;
    }

    public void SpeedUpGame() {
        Time.timeScale = 2f;
        slowed = false;
        speedup = true;
        paused = false;
    }

    public void ResumeGame () {
        if(slowed) SlowMoGame();
        else if (speedup) SpeedUpGame();
        else NormalSpeedGame();
        paused = false;
    }
}
