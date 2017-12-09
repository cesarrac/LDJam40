using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaFiller {
    Area active_area;
    public Cabin cabin_exterior, cabin_interior;
    public List<DirtPatch>dirt_patches {get; protected set;}
    int terminalTileX, terminalTileY;
    public AreaFiller(Area activeArea){
        active_area = activeArea;
        cabin_exterior = new Cabin(Mathf.RoundToInt(active_area.Width * 0.5f), Mathf.RoundToInt(active_area.Height * 0.5f), 
                                   4, 3);
        cabin_interior = new Cabin(Mathf.RoundToInt(active_area.Width * 0.5f), Mathf.RoundToInt(active_area.Height * 0.5f), 
                                6, 5);
        if (active_area.hasFilled == true){
            return;
        }
       
        if (active_area.areaType == AreaType.Exterior){

            FillExterior();
        }
        else{
            terminalTileX = Mathf.RoundToInt(activeArea.Width * 0.5f);
		    terminalTileY = Mathf.RoundToInt(activeArea.Height * 0.5f);
            FillInterior();
        }
        active_area.SetToFilled();
    }

    void FillExterior(){
        dirt_patches = new List<DirtPatch>();
        int map_margin = 5;

         Debug.Log("Filling exterior!");
        // Set the exterior of the interior area in the center of the map
       
  
        for(int x = 0; x < active_area.Width; x++){
            for(int y = 0; y < active_area.Height; y++){
                Tile tile = active_area.tileGrid[x, y];
                if (tile == null)
                    continue;
                // If the tile has already been set, do not re-set!
                if (tile.tileType != TileType.Empty)
                    continue;
                // FIll cabin
                if (x >= cabin_exterior.dimensions.startX && y >= cabin_exterior.dimensions.startY && 
                    x <= cabin_exterior.dimensions.endX && y <= cabin_exterior.dimensions.endY){
                            // Make door
                   /*      if (x == cabinStartX && y == cabinStartY + 2){
                                active_area.tileGrid[x, y].SetAs(TileType.CabinDoor);
                                continue;
                        } */
                        tile.SetAs(TileType.CabExt);
                        continue;
                }
                TileType typeSelected = TileType.Grass;

                if (Random.Range(1, 50) == 1){
                    typeSelected = TileType.Dirt;
                }
                // Set as grass by default:
                tile.SetAs(typeSelected);
                if (typeSelected == TileType.Dirt && 
                    x >= map_margin && x <= active_area.Width - map_margin &&
                    y >= map_margin && y <= active_area.Height - map_margin){
                    FillDirtPatch(x, y);
                }
            }
        }
    }

    void FillDirtPatch(int centerX, int centerY){
        
        int patchWidth = Random.Range(3, 8);
        int patchHeight = Random.Range(3, 7);
     
        int startX = centerX - Mathf.RoundToInt(patchWidth * 0.5f);
        int startY = centerY - Mathf.RoundToInt(patchHeight * 0.5f);
        int lastX = centerX + Mathf.RoundToInt(patchWidth * 0.5f);
        int lastY = centerY + Mathf.RoundToInt(patchHeight * 0.5f);

        DirtPatch newPatch = new DirtPatch(centerX, centerY, patchWidth, patchHeight);
     
        if (dirt_patches.Count >= 1){
            // Make sure we are at a safe distance from the last patch
            Dimensions lastPatchDimensions = dirt_patches[dirt_patches.Count - 1].dimensions;
            if (startX >= lastPatchDimensions.startX && lastX <= lastPatchDimensions.endX){
                return;
            }
        }
        dirt_patches.Add(newPatch);

        for(int x = startX; x <= lastX; x++){
            for(int y = startY; y <= lastY; y++){
                Tile tile = active_area.tileGrid[x, y];
                if (tile == null)
                    continue;
                if (tile.tileType != TileType.Grass && tile.tileType != TileType.Dirt){
                    continue;
                }
                if (x > startX && x < lastX && y > startY && y < lastY){
                    tile.SetAs(TileType.Dirt);
                    // Chance of ORE:
                    FillOre(tile);
                }
                else{
                    tile.SetAs(TileType.Grass_Dirt);
                }
            }
        }
    }

    void FillOre(Tile tile){
        if (Random.Range(1, 10) == 1){
            // Place ore on top of dirt tile:
            ExtractableManager.instance.PlaceExtractable(tile);
        }
    }

    void FillInterior(){
        Debug.Log("Filling interior!");
      
        for(int x = 0; x < active_area.Width; x++){
            for(int y = 0; y < active_area.Height; y++){

                 if (x >= cabin_interior.dimensions.startX && y >= cabin_interior.dimensions.startY && 
                    x <= cabin_interior.dimensions.endX && y <= cabin_interior.dimensions.endY){
                            // Make door
                   /*      if (x == cabinStartX && y == cabinStartY + 2){
                                active_area.tileGrid[x, y].SetAs(TileType.CabinDoor);
                                continue;
                        } */
                        if (x == terminalTileX && y == terminalTileY){
                            active_area.tileGrid[x, y].SetAsTerminal();
                        }
                        active_area.tileGrid[x, y].SetAs(TileType.CabInt);
                        continue;
                }
                 active_area.tileGrid[x, y].SetAs(TileType.Empty);
            }
        }
    }

    void FillFurniture(){
        // for now placing terminal in the center

    }

}

public struct Cabin{
    public Dimensions dimensions;
    public int doorX, doorY;
    public Cabin(int centerX, int centerY, int _width, int _height){
            
            dimensions = new Dimensions(centerX, centerY, _width, _height);
            doorX = dimensions.startX;
            doorY = dimensions.startY + 2;
    }
}
public struct DirtPatch{
    public Dimensions dimensions;
     public DirtPatch(int centerX, int centerY, int _width, int _height){
            
            dimensions = new Dimensions(centerX, centerY, _width, _height);
    }
}

public struct Dimensions{

        public int centerX;
        public int centerY;
        public int width;
        public int height;
        public int startX;
        public int startY;
        public int endX;
        public int endY;

        public Dimensions(int cX, int cY, int _width, int _height){
            
            centerX = cX;
            centerY = cY;
            width = _width;
            height = _height;
            endX = centerX + Mathf.RoundToInt(width * 0.5f);
            endY = centerY  + Mathf.RoundToInt(height * 0.5f);
            startX = centerX - Mathf.RoundToInt(width * 0.5f);
            startY = centerY  - Mathf.RoundToInt(height * 0.5f);;
        }
}