using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour {

	public static SpriteManager instance {get; protected set;}
	Dictionary<string, Sprite> spriteMap;
	public Sprite defaultSprite;
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
	}
	public Sprite GetSprite(string name){
		if (spriteMap.ContainsKey(name) == false){
			return defaultSprite;
		}
		return spriteMap[name];
	}
	public Sprite GetTileSprite(Tile tile){
		
	}
	string GetSuffixFromNeighbor(int x, int y, TileType type, string suffix)
    {
        Tile neighborTile = Area.instance.GetTile(x, y);
        if (neighborTile != null && neighborTile.tileType == type)
            return suffix;


        return string.Empty;
    }
}
