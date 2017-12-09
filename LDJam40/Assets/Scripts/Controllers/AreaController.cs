using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaController : MonoBehaviour {
	public static AreaController instance {get; protected set;}
	Dictionary<Tile, GameObject> tile_GObjs;
	public Area active_area {
		get{return default_areas[activeAreaIndex];}
	}
	int activeAreaIndex = 0;
	Area[] default_areas;
	public AreaFiller area_filler {get; protected set;}
	public CharacterGenerator character_generator {get; protected set;}
	SpriteManager spriteMgr = SpriteManager.instance;
	ObjectPool pool;	
	GameObject tileHolder;
	GameObject terminalGObj;
	
	void Awake(){
		instance = this;
		default_areas = new Area[2];
		default_areas[0] = new Area(50, 50, "Exterior", AreaType.Exterior);
		default_areas[1] = new Area(10, 10, "House", AreaType.Interior);
		
	}
	void Start(){
		pool = ObjectPool.instance;
		spriteMgr = SpriteManager.instance;
		tile_GObjs = new Dictionary<Tile, GameObject>();
		tileHolder = new GameObject();
		tileHolder.name = "Tiles";
		
		
		character_generator = new CharacterGenerator();

		GenerateArea(1, 5, 4);
	
	}

	public void GenerateArea(int areaIndex, int playerStartX, int playerStartY){
		SoundController.Instance.MuteAmbient();
		if (areaIndex >= default_areas.Length || areaIndex < 0)
			areaIndex = 0;

		activeAreaIndex = areaIndex;
		
		if (active_area != null){

			if (active_area.hasFilled == false){
				active_area.Generate();
			}

			area_filler = new AreaFiller(active_area);

			GenerateAreaGObjs();

			// Spawn entities:
			character_generator.SpawnCharacter( playerStartX, playerStartY);
			if (active_area.areaType == AreaType.Exterior){

				EnemyManager.instance.GenerateEnemyNests(active_area);
				//EnemyManager.instance.SpawnEnemies();
				FX_Controller.instance.StartRandomFX();
				SoundController.Instance.PlayAmbient();
			}
			Debug.Log("Area generated: " + active_area.name);
		}
	}
	public void GenerateAreaGObjs(){
		if (tile_GObjs.Count > 0){
			PoolTiles();
			character_generator.PoolCharacter();
			EnemyManager.instance.PoolEnemies();
			FX_Controller.instance.StopRandomFX();
			TerminalController.instance.Pool();
		}
		
		for(int x = 0; x < active_area.Width; x++){
			for(int y = 0; y < active_area.Height; y++){
				Tile tile = active_area.GetTile(x, y);
				if (tile == null)
					continue;
				
				GameObject tileGObj = pool.GetObjectForType("Tile", true, new Vector2(x, y));
				tileGObj.transform.SetParent(tileHolder.transform);
				SpriteRenderer sr = tileGObj.GetComponentInChildren<SpriteRenderer>();
				sr.sprite = spriteMgr.GetTileSprite(x,y,tile.tileType);

				if (tile.tileType == TileType.CabExt){
					CabinExtSpriteCatch(sr, x, y);
				}
				if (tile.tileType == TileType.CabExt || tile.tileType == TileType.CabInt)
					DoDoor(x, y,tileGObj, sr);
				if (tile.hasTerminal){
					DoTerminal(tile);
				}
				if (tile.extractable != null){
					DoExtractables(tile, tileGObj);
				}
				tile_GObjs.Add(tile, tileGObj);
			}
		}
	}
	void DoExtractables(Tile tile, GameObject tileGobj){
		GameObject extractGobj = pool.GetObjectForType("Extractable", true, tile.worldPos);
		extractGobj.GetComponent<Extractable_Controller>().InitExtractable(tile.extractable);
		extractGobj.transform.SetParent(tileGobj.transform);
	}
	void DoDoor(int x,int y, GameObject tileGObj, SpriteRenderer sr){
			Cabin cabin_ext = area_filler.cabin_exterior;
			Cabin cabin_int = area_filler.cabin_interior;
			// Give interactable colliders to doors:
			if (sr.sprite.name == "CabExt_Door"){
				GameObject intCollider = pool.GetObjectForType("Interactable Tile", true, tileGObj.transform.position);
				intCollider.transform.SetParent(tileGObj.transform);
				intCollider.GetComponent<Tile_Interactable>().Init(tileGObj.transform.position);
				intCollider.GetComponent<Tile_Interactable>().SetDoorParams(1, 2, 5);
			}
			else{
				
				if (x == cabin_int.doorX && y == cabin_int.doorY){
					sr.sprite = spriteMgr.GetSprite("CabInt_Door");
					GameObject intCollider = pool.GetObjectForType("Interactable Tile", true, tileGObj.transform.position);
					intCollider.transform.SetParent(tileGObj.transform);
					intCollider.GetComponent<Tile_Interactable>().Init(tileGObj.transform.position);
					intCollider.GetComponent<Tile_Interactable>().SetDoorParams(0, 23, 25);
				}
			}
	}
	void DoTerminal(Tile tile){
		terminalGObj = pool.GetObjectForType("Terminal", true, tile.worldPos);
		terminalGObj.GetComponent<TerminalController>().Init();
		terminalGObj.GetComponent<Terminal_Interactable>().Init(tile.worldPos);
	}

	void CabinExtSpriteCatch(SpriteRenderer spriteRenderer, int x, int y){
		string spriteName = spriteRenderer.sprite.name;
		if (spriteName != "Cabin_NEW" && spriteName != "Cabin_NE" && spriteName != "Cabin_NW"){
			if (spriteName == "CabExt_NES"){
				Cabin cabin_exterior = area_filler.cabin_exterior;
				if (y == cabin_exterior.doorY){
					spriteRenderer.sprite = spriteMgr.GetSprite("CabExt_Door");
				}
			}

			spriteRenderer.sortingLayerName = "Surface";
			spriteRenderer.sortingOrder = 5;
			return;
		}
	
	}
	

	void PoolTiles(){
		foreach(GameObject gobj in tile_GObjs.Values){

			if (gobj.transform.childCount > 1){
				Debug.Log("pool tile children please");
				PoolTileAddOns(gobj);
			}
			// Reset sprite renderer
			SpriteRenderer sr = gobj.GetComponentInChildren<SpriteRenderer>();
			sr.sortingLayerName = "Floor";
			sr.sortingOrder = 0;
			pool.PoolObject(gobj);
		}
		tile_GObjs.Clear();
	}
	void PoolTileAddOns(GameObject tileGobj){
		GameObject[] addOns = new GameObject[tileGobj.transform.childCount - 1];
		if (addOns.Length <= 0)
			return;

		for(int i = 1; i < tileGobj.transform.childCount; i++){
			addOns[i - 1] = tileGobj.transform.GetChild(i).gameObject;
		}
		foreach(GameObject gobj in addOns){
			pool.PoolObject(gobj);
		}
	}
	public GameObject GetTileGObj(Tile tile){
		if (tile_GObjs.ContainsKey(tile) == false)
			return null;
		return tile_GObjs[tile];
	}
}
