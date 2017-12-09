using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MouseInputController : MonoBehaviour {
    public static MouseInputController instance {get; protected set;}
    public delegate void OnMouseClicked(Vector2 mousePosition);
    public event OnMouseClicked onLeftClick;
    public event OnMouseClicked onLeftHold, onLeftUp;
    public event OnMouseClicked onRightClick, onRightHold;

    Vector2 mousePosition;
    void OnEnable(){
        instance = this;
    }
    private void Update()
    {
        
        UpdateMousePosition();
        if (EventSystem.current.IsPointerOverGameObject() == true){
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (onLeftClick != null)
                onLeftClick(mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            if (onLeftHold != null)
            {
                onLeftHold(mousePosition);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (onLeftUp != null)
                onLeftUp(mousePosition);
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (onRightClick != null)
                onRightClick(mousePosition);
        }
        if (Input.GetMouseButton(1))
        {
            if (onRightHold != null)
                onRightHold(mousePosition);
        }
    }

    void UpdateMousePosition()
    {
        Vector3 m = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector2(m.x, m.y);
    }

}
