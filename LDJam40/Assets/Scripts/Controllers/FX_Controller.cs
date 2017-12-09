using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FXType {Bugs, Death}
public class FX_Controller : MonoBehaviour {

	public static FX_Controller instance {get; protected set;}

	ObjectPool pool;

	FXType curFXType;

	List<GameObject> activeFX;

	void Awake(){
		instance = this;
	}
	void Start(){
		activeFX = new List<GameObject>();
	}

	public void DoFX(FXType fxType, Vector2 fxPosition){
		if (pool == null)
			pool = ObjectPool.instance;

		string animStateName = fxType.ToString();
		if (fxType == FXType.Bugs){
			if (AreaController.instance.active_area.areaType == AreaType.Interior)
				return;
				
			animStateName += Random.Range(0, 2).ToString();
		}
		GameObject fxGobj = pool.GetObjectForType("FX", true, fxPosition);
		
		// set animation
		fxGobj.GetComponentInChildren<Animator>().Play(animStateName);
		activeFX.Add(fxGobj);
		
	}

	public void PoolAllFX(){
		if (activeFX.Count > 0){
			foreach(GameObject gobj in activeFX){
				if (gobj.activeSelf == true)
					pool.PoolObject(gobj);
			}
		}
		activeFX.Clear();
	}

	public void StartRandomFX(){
		Debug.Log("Random fx starting");
		StartCoroutine("DoRandomFX");
	}
	public void StopRandomFX(){
		Debug.Log("Random fx stoping");
		StopCoroutine("DoRandomFX");
		PoolAllFX();
	}
	IEnumerator DoRandomFX(){
		while(true){
			GameObject player = AreaController.instance.character_generator.hero_GObj;
			if (player == null){
				break;
			}
			yield return new WaitForSeconds(5);
			if (Random.Range(1, 3) == 1){
				Vector3 distanceFromPlayer = new Vector3(Random.Range(-4, 4), Random.Range(-4, 4), 0);
				DoFX(FXType.Bugs, player.transform.position + distanceFromPlayer);
			}
			yield return null;
		}
	}
}
