using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TileType{
	Empty, Dirt, Grass, Grass_Dirt, CabInt, CabExt, Water
}
public enum ENTERABILITY
{
    Never,
    Soon,
    Yes
}
public class Tile  {
	float movementCost;
	public TileType tileType {get; protected set;}
	public int X {get; protected set;}
	public int Y {get; protected set;}
	public Vector2 worldPos {get; protected set;}
	public Extractable extractable {get; protected set;}

	public bool hasTerminal {get; protected set;}
	public float MovementCost
    {
        get
        {
        	if (tileType == TileType.Empty)
                return 0;
			if (tileType == TileType.CabExt)
                return 0;
			if (tileType == TileType.CabInt)
                return 0;
            if (extractable != null)
                return 0;

            return 1;
        }
    }

	public Tile(int _x, int _y, TileType tType, Vector2 pos, int moveCost = 1){
		X = _x;
		Y = _y;
		tileType = tType;
		worldPos = pos;
		movementCost = 1;
	}
	public ENTERABILITY CanEnter()
    {
        if (movementCost <= 0)
            return ENTERABILITY.Never;
		
        return ENTERABILITY.Yes;
    }

	public void SetAs(TileType newTileType){
		tileType = newTileType;
	}
	public void SetAsTerminal(){
		hasTerminal = true;
	}

	public Tile[] GetNeighbors(bool getDiags = true){
		Tile[] neighbors = new Tile[8];
		if (getDiags == true){
			neighbors[0] = AreaController.instance.active_area.GetTile(X, Y + 1); // N
			neighbors[1] = AreaController.instance.active_area.GetTile(X + 1, Y + 1); // NE
			neighbors[2] = AreaController.instance.active_area.GetTile(X + 1, Y); // E
			neighbors[3] = AreaController.instance.active_area.GetTile(X + 1, Y - 1); // SE
			neighbors[4] = AreaController.instance.active_area.GetTile(X, Y - 1); // S
			neighbors[5] = AreaController.instance.active_area.GetTile(X - 1, Y - 1); // SW
			neighbors[6] = AreaController.instance.active_area.GetTile(X - 1, Y); // W
			neighbors[7] = AreaController.instance.active_area.GetTile(X - 1, Y + 1);	// NW
		}
		else{
			neighbors = new Tile[4];
			neighbors[0] = AreaController.instance.active_area.GetTile(X, Y + 1); // N
			neighbors[1] = AreaController.instance.active_area.GetTile(X + 1, Y); // E
			neighbors[2] = AreaController.instance.active_area.GetTile(X, Y - 1); // S
			neighbors[3] = AreaController.instance.active_area.GetTile(X - 1, Y); // W
		}
		  return neighbors.Where(tile => tile != null).ToArray();
	}

	public bool PlaceExtractable(Extractable newExtractable){
		if (extractable != null){
			return false;
		}
		extractable = newExtractable;
		return true;
	}
	public bool RemoveExtractable(){
		if (extractable == null){
			return false;
		}
		extractable = null;
		return true;
	}
}
