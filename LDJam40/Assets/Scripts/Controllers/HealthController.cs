using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {

	float starting_hitpoints;
	float pHitpoints;
	float hitPoints {
		get{return pHitpoints;}
		set{pHitpoints = Mathf.Clamp(value, 0, starting_hitpoints);}
	}
	public float HitPoints{get {return hitPoints;}}
	public delegate void HPZero(GameObject gObj);
	public event HPZero onHPZero;
	public delegate void Death();
	public event Death onDeath;
	public bool isPlayer = false;
	public void Init(float hPAtStart, bool isplayer){
		starting_hitpoints = hPAtStart;
		hitPoints = starting_hitpoints;
		Debug.Log("Health-control initialized with " + hPAtStart + " hp");
		isPlayer = isplayer;
	}
	public void ReceiveDamage(float dmg){
		hitPoints -= dmg;
		if (hitPoints <= 0){
			if (onHPZero != null){
				onHPZero(this.gameObject);
				return;
			}
			if (onDeath != null){
				onDeath();
			}
		}
		else{
			if (isPlayer)
				InventoryUI.instance.ChangeHealthBar(hitPoints, starting_hitpoints);
		}
	}
}
