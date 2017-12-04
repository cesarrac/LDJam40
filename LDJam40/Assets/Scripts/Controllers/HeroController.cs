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
	AreaController areaController;
	public Inventory heroInventory {get; protected set;}
	RadiationTracker radiation_tracker;
	float speed = 1;
	bool isMoving = false;
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

	public void Init(Inventory inventory, float moveSpeed){
		speed = moveSpeed;
		areaController = AreaController.instance;
		areaWidth = areaController.active_area.Width - 1;
		areaHeight = areaController.active_area.Height - 1;
		mouse_controller = MouseInputController.instance;
		mouse_controller.onRightClick += TryInteract;
		key_controller = KeyInputController.instance;
		key_controller.onKeyPressed += OnMove;
		key_controller.onKeyHeld += Move;
		key_controller.onKeyUp += OnMoveStop;
		key_controller.onInteractBttnPressed += TryInteract;
		SetAnimParams(0, 0);
		Camera_Controller.instance.SetTargetAndLock(this.transform, 0, areaWidth, 0, areaHeight);
		OnChangeArea();
		heroInventory = inventory;
		radiation_tracker = new RadiationTracker(heroInventory);
		InventoryUI.instance.Init(heroInventory, radiation_tracker);
		move_x = transform.position.x;
		move_y = transform.position.y;
		GetComponent<HeroAttackController>().Init();
	}

	public void OnChangeArea(){
		curAreaType = areaController.active_area.areaType;
		if (curAreaType == AreaType.Exterior)
			cabin_exterior = areaController.area_filler.cabin_exterior;
		else
			cabin_interior = areaController.area_filler.cabin_interior;
	//	Debug.Log("Hero reads from AreaController that active area is : " + curAreaType);
	}



	void OnMove(){
		anim.ResetTrigger("hero_Idle");
		anim.SetTrigger("hero_Walk");
		isMoving = true;
	}
	void Move(){
		float input_x = Input.GetAxisRaw("Horizontal");
		float input_y = Input.GetAxisRaw("Vertical");
		transform.position += new Vector3(input_x, input_y, 0) * speed * Time.deltaTime;
		ClampPosition();
		SetAnimParams(input_x, input_y);
	}
	void OnMoveStop(){
		if (isMoving == true){
			anim.SetTrigger("hero_Idle");
			isMoving = false;
		}
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
	void TryInteract(){
		// Get tile under player
		Tile tileUnderMe = areaController.active_area.GetTile(transform.position);
		if (tileUnderMe == null)
			return;
		GameObject tileGobj = areaController.GetTileGObj(tileUnderMe);
		if (tileGobj == null)
			return;
		Interactable interactable = null;

		while(interactable == null){

			interactable = tileGobj.GetComponentInChildren<Interactable>();

			if (interactable == null){
				// try its neighbors
				Tile[] neighbors = tileUnderMe.GetNeighbors();
				for(int i = 0; i < neighbors.Length; i++){
					if (neighbors[i] == null)
						continue;
					tileGobj = areaController.GetTileGObj(neighbors[i]);
					if (tileGobj == null)
						continue;

					interactable = tileGobj.GetComponentInChildren<Interactable>();
					
					if (interactable != null)
						break; // FOUND ONE
				}

				// if nothing was found, break!
				break;
			}
		}

		if (interactable != null)
			interactable.TryInteract(this.gameObject);
	
	}

	void OnDisable(){
		if (key_controller != null){
			key_controller.onKeyPressed -= OnMove;
			key_controller.onKeyHeld -= Move;
			key_controller.onKeyUp -= OnMoveStop;
			key_controller.onInteractBttnPressed -= TryInteract;
		}
		if (mouse_controller != null){
			mouse_controller.onRightClick -= TryInteract;
		}
		if (radiation_tracker != null){
			if (heroInventory != null)
				heroInventory.onInventoryChanged -= radiation_tracker.OnRadiationReceived;
		}
	}
}
