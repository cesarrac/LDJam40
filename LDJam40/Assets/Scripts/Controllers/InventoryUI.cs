using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventoryUI : MonoBehaviour {
	public static InventoryUI instance {get; protected set;}
	Inventory active_inventory;
	Text oreCount;
	Animator invUIAnim, radUIAnim;
	public GameObject invUIGobj;
	public GameObject radUIGobj;
	 Text radLevelText;
	 public Fade mainMenuFade, menuPanelFade;
	 public GameObject mainMenu;
	 bool mainMenuFaded = false;
	 public RectTransform healthbar;
	void Awake(){
		instance = this;
		oreCount = invUIGobj.GetComponentInChildren<Text>();
		radUIAnim = radUIGobj.GetComponentInChildren<Animator>();
		invUIAnim = invUIGobj.GetComponent<Animator>();
		radLevelText = radUIGobj.GetComponentInChildren<Text>();
	}
	public void Init(Inventory inv, RadiationTracker radTrack){
		active_inventory = inv;
		// Remove first since UI is not being pooled
		active_inventory.onInventoryChanged -= OnInventoryChanged;
		radTrack.onRadChange -= OnRadChange;

		active_inventory.onInventoryChanged += OnInventoryChanged;
		radTrack.onRadChange += OnRadChange;

		// to hide main menu set this key press
		if (mainMenuFaded == false)
			KeyInputController.instance.onInteractBttnPressed += FadeMainMenu;
	}
	void FadeMainMenu(){

		KeyInputController.instance.onInteractBttnPressed -= FadeMainMenu;
		menuPanelFade.onFadeDone += HideMainMenu;
		mainMenuFade.FadeOut();
		menuPanelFade.FadeOut();
		mainMenuFaded = true;
	}
	void HideMainMenu(){
		menuPanelFade.onFadeDone -= HideMainMenu;

		mainMenu.SetActive(false);
	}
	public void FadeInRestartMenu(){
		mainMenu.SetActive(true);
		KeyInputController.instance.onInteractBttnPressed += RestartGame;
		mainMenuFade.FadeIn();
		menuPanelFade.FadeIn();
	}
	public void RestartGame(){
		SceneManager.LoadScene(0);
	}
	void OnInventoryChanged(int newCount){
		oreCount.text = newCount.ToString();
		invUIAnim.SetTrigger("oreReceived");
	}
	void OnRadChange(){
		
		radUIAnim.SetTrigger("onRadChanged");
		radLevelText.text = RadiationTracker.instance.radLevel.ToString();
		
	}
	void OnDisable(){
		if (active_inventory != null){
			active_inventory.onInventoryChanged -= OnInventoryChanged;
		}
		if (RadiationTracker.instance != null)
			RadiationTracker.instance.onRadChange -= OnRadChange;
	}
	public void ChangeHealthBar(float hitPoints, float maxHP){
		float diff = maxHP - hitPoints;
		healthbar.localScale = new Vector3(diff * 0.1f, 1, 1);
	}
}
