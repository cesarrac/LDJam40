using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType{
	Empty, Dirt, Grass, Grass_Dirt, CabInt, CabExt, Water
}
public class Tile  {

	public TileType tileType {get; protected set;}
	public int x {get; protected set;}
	public int y {get; protected set;}
	public Vector2 worldPos {get; protected set;}

	public Tile(int _x, int _y, TileType tType, Vector2 pos){
		x = _x;
		y = _y;
		tileType = tType;
		worldPos = pos;
	}

	public void SetAs(TileType newTileType){
		tileType = newTileType;
	}

	public Tile[] GetNeighbors(bool getDiags = true){
		Tile[] neighbors = new Tile[8];
		if (getDiags == true){
			neighbors[0] = AreaController.instance.active_area.GetTile(x, y + 1); // N
			neighbors[1] = AreaController.instance.active_area.GetTile(x + 1, y + 1); // NE
			neighbors[2] = AreaController.instance.active_area.GetTile(x + 1, y); // E
			neighbors[3] = AreaController.instance.active_area.GetTile(x + 1, y - 1); // SE
			neighbors[4] = AreaController.instance.active_area.GetTile(x, y - 1); // S
			neighbors[5] = AreaController.instance.active_area.GetTile(x - 1, y - 1); // SW
			neighbors[6] = AreaController.instance.active_area.GetTile(x - 1, y); // W
			neighbors[7] = AreaController.instance.active_area.GetTile(x - 1, y + 1);	// NW
		}
		else{
			neighbors = new Tile[4];
			neighbors[0] = AreaController.instance.active_area.GetTile(x, y + 1); // N
			neighbors[1] = AreaController.instance.active_area.GetTile(x + 1, y); // E
			neighbors[2] = AreaController.instance.active_area.GetTile(x, y - 1); // S
			neighbors[3] = AreaController.instance.active_area.GetTile(x - 1, y); // W
		}
		return neighbors;
	}
}
