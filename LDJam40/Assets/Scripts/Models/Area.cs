using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area  {
	public int width{get;protected set;}
	public int height{get; protected set;}
	public static Area instance {get; protected set;}
	public Tile[,] tileGrid {get; protected set;}
	public Area(int _width, int _height){
		instance = this;
		width = _width;
		height = _height;
	}
	public void Generate(int _width = 0, int _height = 0){
		// Re-set width if params set
		if (_width > 0 && _height > 0){
			width = _width;
			height = _height;
		}
	
		InitTileGrid();
	}
	void InitTileGrid(){
		tileGrid = new Tile[width, height];
		for(int x = 0; x < width; x++){
			for(int y = 0; y < height; y++){
				tileGrid[x, y] = new Tile(x, y, TileType.Empty, new Vector2(x, y));
		 	}
		}
	}

	public Tile GetTile(int x, int y){
		if (x >= 0 && x <= width && y >= 0 && y <= height){
			return tileGrid[x, y];
		}
		return null;
	}
}

