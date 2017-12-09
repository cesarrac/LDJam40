using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

	public float speed = 4;
	Vector3 travelDirection = Vector2.zero;
	Animator anim;
	CombatController combat_controller;
	void OnEnable(){
		anim = GetComponentInChildren<Animator>();
	}
	// Use this for initialization
	public void Init(Vector3 direction, CombatController combatCont){
		combat_controller = combatCont;
		travelDirection = direction;
		Debug.Log("Shot projectile towards " + direction);
	}

	private void Update(){

		transform.position += travelDirection * speed * Time.deltaTime;
	}

	void OnDisable(){
		
		GetComponent<BoxCollider2D>().enabled = true;
	}
	void OnTriggerEnter2D(Collider2D coll){
		if (coll.GetComponentInParent<CombatController>() != null && combat_controller != null){
			travelDirection = Vector2.zero;
				// play explosion
			anim.SetTrigger("hit");
			combat_controller.DoDamage(coll.GetComponentInParent<CombatController>());
			GetComponent<BoxCollider2D>().enabled = false;
			GetComponent<WaitToPoolController>().ResetTo(0.5f);
		}
	}
}
