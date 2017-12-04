using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour {

	public static SpriteManager instance {get; protected set;}
	Dictionary<string, Sprite> spriteMap;
	public Sprite defaultSprite;
	AreaController areaController;
	int totalOreSprites = 0;
	void Awake(){
		instance = this;
		Init();
	}
	void Init(){
		spriteMap = new Dictionary<string, Sprite>();
		Sprite[] tiles = Resources.LoadAll<Sprite>("Sprites/Tiles/Rad-Tiles");
		for(int i = 0; i < tiles.Length; i++){
			spriteMap.Add(tiles[i].name, tiles[i]);
		}
		Sprite[] ore = Resources.LoadAll<Sprite>("Sprites/Tiles/Ore");
		for(int i = 0; i < ore.Length; i++){
			spriteMap.Add(ore[i].name, ore[i]);
		}
		totalOreSprites = ore.Length;
	}
	void Start(){
		areaController = AreaController.instance;
	}
	public Sprite GetSprite(string name){
		if (name == null)
			return defaultSprite;

		if (spriteMap.ContainsKey(name) == false){
			//Debug.Log("Sprite Manager could not find sprite: " + name);
			return defaultSprite;
		}
		return spriteMap[name];
	}
	public Sprite GetTileSprite(int x, int y, TileType tileType){
		string spriteName = tileType.ToString() + "_";
		if (areaController == null){
			areaController = AreaController.instance;
		}
		if (tileType == TileType.Empty){
			return GetSprite(spriteName);
		}
        string suffix = string.Empty;
        suffix += GetSuffixFromNeighbor(x, y + 1, tileType, "N");
        suffix += GetSuffixFromNeighbor(x + 1, y, tileType, "E");
        suffix += GetSuffixFromNeighbor(x, y - 1, tileType, "S");
        suffix += GetSuffixFromNeighbor(x - 1, y, tileType, "W");
		
        spriteName += suffix;
		if (tileType == TileType.Grass_Dirt){
			spriteName = DirtPatchSpriteCatch(spriteName, x, y);
		}
		return GetSprite(spriteName);
	}
	string DirtPatchSpriteCatch(string spriteName, int x, int y){
		if (spriteName == "Grass_Dirt_NSW"){
			return "Grass_Dirt_NS_W";
		}
		if (spriteName == "Grass_Dirt_NES"){
			return "Grass_Dirt_NS_E";
		}
		if (spriteName == "Grass_Dirt_NEW"){
			 return "Grass_Dirt_EW_S";
		}
		if (spriteName == "Grass_Dirt_ESW"){
			 return "Grass_Dirt_ES";
		}
	
		if (spriteName == "Grass_Dirt_NS" || spriteName == "Grass_Dirt_S" || spriteName == "Grass_Dirt_W"){
			if (areaController.active_area.tileGrid[x + 1, y].tileType == TileType.Dirt ){
				return "Grass_Dirt_NS_W";
			}else{
				return "Grass_Dirt_NS_E";
			}
		}
		if (spriteName == "Grass_Dirt_EW"){
			if (areaController.active_area.tileGrid[x, y - 1].tileType == TileType.Dirt){
				return "Grass_Dirt_EW_N";
			}else{
				return "Grass_Dirt_EW_S";
			}
		}
		return spriteName;
	}
	string GetSuffixFromNeighbor(int x, int y, TileType type, string suffix)
    {
		TileType altType = type;
		if (type == TileType.Dirt || type == TileType.Grass)
			return string.Empty;
	/* 	if (type == TileType.Grass_Dirt){
			altType = TileType.Grass;
		} */
        Tile neighborTile = areaController.active_area.GetTile(x, y);
        if (neighborTile != null){
			if (neighborTile.tileType == type){
            	return suffix;
			}
		/* 	else if (neighborTile.tileType == altType)
				return suffix; */
		}


        return string.Empty;
    }
	public Sprite GetOreSprite(){
		string oreName = "Ore_" + Random.Range(0, totalOreSprites);
		//Debug.Log("Getting sprite " + oreName);
		return GetSprite(oreName);
	}
}
