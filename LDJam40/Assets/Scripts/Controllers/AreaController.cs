using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaController : MonoBehaviour {
	public List<GameObject> tile_GObjs;
	public Area active_area {get; protected set;}

	ObjectPool pool;

	void Start(){
		pool = ObjectPool.instance;
		tile_GObjs = new List<GameObject>();
		active_area = new Area(50, 50);
		Generate();
	}
	public void Generate(){
			// Test:
		if (active_area != null){

			active_area.Generate();
			new AreaFiller(active_area);
			GenerateAreaGObjs();
		}
	}
	public void GenerateAreaGObjs(){
		if (tile_GObjs.Count > 0){
			PoolTiles();
		}
		SpriteManager spriteMgr = SpriteManager.instance;
		for(int x = 0; x < active_area.width; x++){
			for(int y = 0; y < active_area.height; y++){
				GameObject tileGObj = pool.GetObjectForType("Tile", true, new Vector2(x, y));
				tileGObj.GetComponentInChildren<SpriteRenderer>().sprite = spriteMgr.GetSprite(active_area.tileGrid[x, y].tileType.ToString());
				tile_GObjs.Add(tileGObj);
			}
		}
	}

	void PoolTiles(){
		foreach(GameObject gobj in tile_GObjs){
			pool.PoolObject(gobj);
		}
		tile_GObjs.Clear();
	}
}
