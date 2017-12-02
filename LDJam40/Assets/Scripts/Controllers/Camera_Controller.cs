using UnityEngine;
using System.Collections;

public enum CameraMode
{
    Locked,
    Free,
  
}
public class Camera_Controller : MonoBehaviour {

    public CameraMode CamMode { get; protected set; }

    public static Camera_Controller instance { get; protected set; }

    Transform target;

    public float dampTime = 0.15f, folMouseDampTime;
    float followDampTime;
    private Vector3 velocity = Vector3.zero;

    public float offsetY = 0;
    Vector3 offsetV2;

    float camXPosMin = 0, camXPosMax = 0;
    float camYPosMin = 0, camYPosMax = 0;

    void Awake()
    {
        instance = this;
        CamMode = CameraMode.Free;
        followDampTime = dampTime;
        folMouseDampTime = 1.0f;
        offsetV2 = new Vector3(0, offsetY, 0);
    }

    public void SetTargetAndLock(Transform t, float xMIn, float xMax, float yMin, float yMax)
    {
        camXPosMin = xMIn;
        camXPosMax = xMax;
        camYPosMin = yMin;
        camYPosMax = yMax;
        target = t;
        CamMode = CameraMode.Locked;

    }

    public void SetFree()
    {
        target = null;
        CamMode = CameraMode.Free;
    }


    void Update()
    {
        if (CamMode == CameraMode.Locked && target)
        {
            MoveToTarget();
        }
        

        if (Input.GetMouseButtonUp(2))
        {
            CamMode = CameraMode.Locked;
        }

    }

    public void OnAreaChanged(int width, int height)
    {
        // To get the Camera's X position min/max:
        // min x = area's width / camera's size
        // max x = area's width - (area's width / camera's size)
        camXPosMin = width / Camera.main.orthographicSize;
        camXPosMax = width - (camXPosMin);

        // To get the Camera's Y position min/max:
        // min y = area's height / camera's size
        // max x = height - area's height / camera's size
        camYPosMin = height / Camera.main.orthographicSize;
        camYPosMax = (height - (camYPosMin)) + Camera.main.orthographicSize;
    }

    public void SwitchCamMode(CameraMode _mode)
    {
        CamMode = _mode;
    }

    void MoveToTarget()
    {
        dampTime = followDampTime;

        //Vector3 targetPos = target.position;
        //targetPos.x = Mathf.Ceil(target.position.x);
        //targetPos.y = Mathf.Ceil(target.position.y);

        Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
        Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z) + offsetV2); //(new Vector3(0.5, 0.5, point.z));
        Vector3 destination = transform.position + delta;

        destination.x = Mathf.Clamp(destination.x, camXPosMin, camXPosMax);
        destination.y = Mathf.Clamp(destination.y, camYPosMin, camYPosMax);


        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
    }
}
