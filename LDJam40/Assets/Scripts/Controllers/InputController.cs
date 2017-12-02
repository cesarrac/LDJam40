using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

	public static InputController instance {get; protected set;}
	public delegate void OnKey();
	public event OnKey onKeyPressed;
	public event OnKey onKeyHeld;
	public event OnKey onKeyUp;

	void Awake(){
		instance = this;
	}

	void Update(){
		if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) ||
			Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)){
			if (onKeyPressed != null){
				onKeyPressed();
			}
		}
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ||
			Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)){
			if (onKeyHeld != null){
				onKeyHeld();
			}
		}
		if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) ||
			Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)){
			if (onKeyUp != null){
				onKeyUp();
			}
		}
	}
}
