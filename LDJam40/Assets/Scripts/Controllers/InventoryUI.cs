using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {
	public static InventoryUI instance {get; protected set;}
	Inventory active_inventory;
	Text oreCount;
	Animator invUIAnim;
	public GameObject invUIGobj;
	public GameObject radUIGobj;
	 Text radLevelText;
	void Awake(){
		instance = this;
		oreCount = invUIGobj.GetComponentInChildren<Text>();
		invUIAnim = invUIGobj.GetComponent<Animator>();
		radLevelText = radUIGobj.transform.GetChild(1).gameObject.GetComponent<Text>();
	}
	public void Init(Inventory inv, RadiationTracker radTrack){
		active_inventory = inv;
		// Remove first since UI is not being pooled
		active_inventory.onInventoryChanged -= OnInventoryChanged;
		radTrack.onRadChange -= OnRadChange;

		active_inventory.onInventoryChanged += OnInventoryChanged;
		radTrack.onRadChange += OnRadChange;
	}
	void OnInventoryChanged(int newCount){
		oreCount.text = newCount.ToString();
		if (invUIAnim != null){
			invUIAnim.SetTrigger("oreReceived");
		}
	}
	void OnRadChange(){
		/* if (invUIAnim != null){
			invUIAnim.SetTrigger("onRadChange");
		} */
		radLevelText.text = RadiationTracker.instance.radLevel.ToString();
	}
	void OnDisable(){
		if (active_inventory != null){
			active_inventory.onInventoryChanged -= OnInventoryChanged;
		}
		if (RadiationTracker.instance != null)
			RadiationTracker.instance.onRadChange -= OnRadChange;
	}
}
