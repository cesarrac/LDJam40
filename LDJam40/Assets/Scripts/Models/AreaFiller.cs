using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaFiller {
    Area active_area;
    public AreaFiller(Area activeArea){
        active_area = activeArea;

         FillExterior();
    }

    void FillExterior(){
        // Set the exterior of the interior area in the center of the map
        int cabinX = Mathf.RoundToInt(active_area.width * 0.5f);
        int cabinY = Mathf.RoundToInt(active_area.height * 0.5f);
        int cabinWidth = 3;
        int cabinHeight = 3;
        int cabinStartX = cabinX - cabinWidth;
        int cabinStartY = cabinY - cabinHeight;
        int cabinEndX = cabinX + cabinWidth;
        int cabinEndY = cabinY + cabinHeight;

        for(int x = 0; x < active_area.width; x++){
            for(int y = 0; y < active_area.height; y++){
                // FIll cabin
                if (x >= cabinStartX && y >= cabinStartY && 
                    x <= cabinEndX && y <= cabinEndY){
                        active_area.tileGrid[x, y].SetAs(TileType.Cabin);
                        continue;
                }
                TileType typeSelected = TileType.Grass;
                if (Random.Range(1, 10) == 1){
                    typeSelected = TileType.Dirt;
                }
                // Set as dirt:
                 active_area.tileGrid[x, y].SetAs(typeSelected);

            }
        }
    }

}