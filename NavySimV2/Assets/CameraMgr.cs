using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMgr : MonoBehaviour
{
    public static CameraMgr inst;
    private void Awake()
    {
        inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public GameObject RTSCameraRig;
    public GameObject YawNode;   // Child of RTSCameraRig
    public GameObject PitchNode; // Child of YawNode
    public GameObject RollNode;  // Child of PitchNode
    public Camera camera;
    //Camera is child of RollNode

    public float cameraMoveSpeed = 100;
    public float cameraTurnRate = 10;
    public Vector3 currentYawEulerAngles = Vector3.zero;
    public Vector3 currentPitchEulerAngles = Vector3.zero;

    public float deltaTime;
    bool spedUp = false;
    // Update is called once per frame
    void Update()
    {
        if (!spedUp)
            deltaTime = Time.deltaTime;

        else
            deltaTime = Time.deltaTime / ControlMgr.inst.timeFactor;
            
        if(Input.GetKeyUp(KeyCode.T))
            spedUp = !spedUp;

        if (Input.GetKey(KeyCode.W))
            YawNode.transform.Translate(Vector3.forward * deltaTime * cameraMoveSpeed);
        if (Input.GetKey(KeyCode.S))
            YawNode.transform.Translate(Vector3.back * deltaTime * cameraMoveSpeed);
        if (Input.GetKey(KeyCode.A))
            YawNode.transform.Translate(Vector3.left * deltaTime * cameraMoveSpeed);
        if (Input.GetKey(KeyCode.D))
            YawNode.transform.Translate(Vector3.right * deltaTime * cameraMoveSpeed);
        if (Input.GetKey(KeyCode.R))
            YawNode.transform.Translate(Vector3.up * deltaTime * cameraMoveSpeed);
        if (Input.GetKey(KeyCode.F))
            YawNode.transform.Translate(Vector3.down * deltaTime * cameraMoveSpeed);

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (cameraMoveSpeed == 100)
                cameraMoveSpeed = 500;
            else
                cameraMoveSpeed = 100;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
            camera.fieldOfView--;
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
            camera.fieldOfView++;

        camera.fieldOfView = Utils.Clamp(camera.fieldOfView, 10, 100);

        currentYawEulerAngles = YawNode.transform.localEulerAngles;
        if (Input.GetKey(KeyCode.Q))
            currentYawEulerAngles.y -= cameraTurnRate * deltaTime;
        if (Input.GetKey(KeyCode.E))
            currentYawEulerAngles.y += cameraTurnRate * deltaTime;
        YawNode.transform.localEulerAngles = currentYawEulerAngles;

        currentPitchEulerAngles = PitchNode.transform.localEulerAngles;
        if (Input.GetKey(KeyCode.Z))
            currentPitchEulerAngles.x -= cameraTurnRate * deltaTime;
        if (Input.GetKey(KeyCode.X))
            currentPitchEulerAngles.x += cameraTurnRate * deltaTime;
        PitchNode.transform.localEulerAngles = currentPitchEulerAngles;

        if (Input.GetKeyUp(KeyCode.C)) {
            if (isRTSMode) {
                YawNode.transform.SetParent(SelectionMgr.inst.selectedEntity.cameraRig.transform);
                YawNode.transform.localPosition = Vector3.zero;
                YawNode.transform.localEulerAngles = Vector3.zero;
            } else {
                YawNode.transform.SetParent(RTSCameraRig.transform);
                YawNode.transform.localPosition = Vector3.zero;
                YawNode.transform.localEulerAngles = Vector3.zero;
            }
            isRTSMode = !isRTSMode;
        }

        if (Input.GetKeyUp(KeyCode.BackQuote))
        {
            YawNode.transform.localEulerAngles = Vector3.zero;
            PitchNode.transform.localEulerAngles = Vector3.zero;
            camera.fieldOfView = 60;
        }




    }
    public bool isRTSMode = true;
}
