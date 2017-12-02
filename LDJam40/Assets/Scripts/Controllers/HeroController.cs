using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour {

	Animator anim;
	float pMoveX, pMoveY;
	int areaWidth = 100, areaHeight = 100;
	InputController input_controller;
	float move_x {
		get{return pMoveX;}
		set{
			pMoveX = Mathf.Clamp(value, 0, areaWidth);
		}
	}
	float move_y{
		get{return pMoveY;}
		set{
			pMoveY = Mathf.Clamp(value, 0, areaHeight);
		}
	}

	void Awake(){
		anim = GetComponentInChildren<Animator>();
	}

	void Start(){
		areaWidth = Area.instance.width - 1;
		areaHeight = Area.instance.height - 1;

		input_controller = InputController.instance;
		input_controller.onKeyPressed += OnMove;
		input_controller.onKeyHeld += Move;
		input_controller.onKeyUp += OnMoveStop;
		SetAnimParams(0, 0);
		Camera_Controller.instance.SetTargetAndLock(this.transform, 0, areaWidth, 0, areaHeight);
	}

	void Update(){
		 
		
	}

	void OnMove(){
		anim.SetTrigger("hero_Walk");
	}
	void Move(){
		float input_x = Input.GetAxisRaw("Horizontal");
		float input_y = Input.GetAxisRaw("Vertical");
		transform.position += new Vector3(input_x, input_y, 0) * 3 * Time.deltaTime;
		ClampPosition();
		SetAnimParams(input_x, input_y);
	}
	void OnMoveStop(){
		anim.SetTrigger("hero_Idle");
	}

	void ClampPosition(){
		move_x = transform.position.x;
		move_y = transform.position.y;
		transform.position = new Vector2(move_x, move_y);
	}

	void SetAnimParams(float x, float y){
		anim.SetFloat("x", x);
		anim.SetFloat("y", y);
	}
}
