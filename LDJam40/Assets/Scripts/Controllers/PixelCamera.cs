using UnityEngine;
using System.Collections;


public class PixelCamera : MonoBehaviour {

    public static PixelCamera instance { get; protected set; }

    public bool isPixelPerfect = false;
    public float targetResolution = 1080;
    public float pixPerUnit = 32;
    public float PPUScale = 1;


    void OnEnable()
    {
        instance = this;


        targetResolution = Screen.height;
        //if (targetResolution >= 1080)
        //{
        //    PPUScale = 1;
        //}
        //else
        //    PPUScale = 2;

        if (isPixelPerfect == true)
            SetPixelCamSize();

    }

    void SetPixelCamSize()
    {
        float size = ((targetResolution) / (PPUScale * pixPerUnit)) * 0.5f;
        Camera.main.orthographicSize = size;
    }

    public void SwitchZoom()
    {
        if (PPUScale == 1)
            PPUScale = 2;
        else if (PPUScale == 2)
            PPUScale = 4;
        else
            PPUScale = 1;

        if (isPixelPerfect == true)
            SetPixelCamSize();
    }

    public void ZoomTo(float scale)
    {
        // Do not allow an odd number scale (3 in this case)
        float scaleClamped = scale % 2 == 0 ? Mathf.Clamp(scale, 1, 4) : 1;

        PPUScale = scaleClamped;

        SetPixelCamSize();

    }
}
