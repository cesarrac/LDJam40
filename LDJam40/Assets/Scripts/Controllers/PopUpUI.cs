using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class PopUpUI : MonoBehaviour, IPointerEnterHandler {
    public void OnPointerEnter(PointerEventData eventData)
    {
        MissionUIController.instance.DoPopUp();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
