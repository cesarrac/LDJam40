using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour {

	Animator anim;
	float pMoveX, pMoveY;
	int areaWidth = 100, areaHeight = 100;
	KeyInputController key_controller;
	MouseInputController mouse_controller;
	Cabin cabin_interior, cabin_exterior;
	AreaType curAreaType;
	public LayerMask interactableMask;
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

	void OnEnable(){
		anim = GetComponentInChildren<Animator>();
	}

	public void Init(){
		areaWidth = AreaController.instance.active_area.width - 1;
		areaHeight = AreaController.instance.active_area.height - 1;
		mouse_controller = MouseInputController.instance;
		mouse_controller.onRightClick += TryInteract;
		key_controller = KeyInputController.instance;
		key_controller.onKeyPressed += OnMove;
		key_controller.onKeyHeld += Move;
		key_controller.onKeyUp += OnMoveStop;
		SetAnimParams(0, 0);
		Camera_Controller.instance.SetTargetAndLock(this.transform, 0, areaWidth, 0, areaHeight);
		OnChangeArea();

		move_x = transform.position.x;
		move_y = transform.position.y;
	}

	public void OnChangeArea(){
		curAreaType = AreaController.instance.active_area.areaType;
		if (curAreaType == AreaType.Exterior)
			cabin_exterior = AreaController.instance.area_filler.cabin_exterior;
		else
			cabin_interior = AreaController.instance.area_filler.cabin_interior;
		Debug.Log("Hero reads from AreaController that active area is : " + curAreaType);
	}



	void OnMove(){
		anim.ResetTrigger("hero_Idle");
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
		if (curAreaType == AreaType.Exterior){
			if (move_x >= cabin_exterior.dimensions.startX && move_x <= cabin_exterior.dimensions.endX &&
				move_y >= cabin_exterior.dimensions.startY && move_y <= cabin_exterior.dimensions.endY){
				move_x = cabin_exterior.doorX;
				move_y = cabin_exterior.doorY;
			}
		}
		else{
			if (move_x <= cabin_interior.dimensions.startX){
				move_x = cabin_interior.dimensions.startX;
			}
			else if (move_x >= cabin_interior.dimensions.endX){
				move_x = cabin_interior.dimensions.endX;
			}
			if (move_y <= cabin_interior.dimensions.startY){
				move_y = cabin_interior.dimensions.startY;
			}
			else if (move_y >= cabin_interior.dimensions.endY - 1){
				move_y = cabin_interior.dimensions.endY - 1;
			}
		}
	
		transform.position = new Vector2(move_x, move_y);
	}

	void SetAnimParams(float x, float y){
		anim.SetFloat("x", x);
		anim.SetFloat("y", y);
	}

 void TryInteract(Vector2 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, 0, interactableMask);
		Debug.Log("shooting ray");
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.GetComponentInParent<Interactable>() != null)
            {
                hit.collider.gameObject.GetComponentInParent<Interactable>().TryInteract(this.gameObject);
            }
        
        }
    }

	void OnDisable(){
		if (key_controller != null){
			key_controller.onKeyPressed -= OnMove;
			key_controller.onKeyHeld -= Move;
			key_controller.onKeyUp -= OnMoveStop;
		}
		if (mouse_controller != null){
			mouse_controller.onRightClick -= TryInteract;
		}
	}
}
