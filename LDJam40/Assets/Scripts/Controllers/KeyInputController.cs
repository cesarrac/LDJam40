using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInputController : MonoBehaviour {

	public static KeyInputController instance {get; protected set;}
	public delegate void OnKey();
	public event OnKey onKeyPressed;
	public event OnKey onKeyHeld;
	public event OnKey onKeyUp;
	int[] keysPressed = new int[4];
	void Awake(){
		instance = this;
	}

	void Update(){
		
		if (Input.GetKeyDown(KeyCode.W)){
			keysPressed[0] = 1;
		}
		if (Input.GetKeyDown(KeyCode.A)){
			keysPressed[1] = 1;
		}
		if (Input.GetKeyDown(KeyCode.S)){
			keysPressed[2] = 1;
		}
		if (Input.GetKeyDown(KeyCode.D)){
			keysPressed[3] = 1;
		}

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
	/* 	if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) ||
			Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)){
				if (onKeyUp != null){
					onKeyUp();
				}
		}  */
		if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0){
			if (onKeyUp != null){
					onKeyUp();
			}
		}
	
	}


}
