using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Interactable : Interactable {

    public int areaIndexToLoad = 0;
    public int playerStartOnLoadX = 25;
    public int playerStartOnLoadY = 25;

    public void SetDoorParams(int indexToLoad, int playerX, int playerY){
        areaIndexToLoad = indexToLoad;
        playerStartOnLoadX = playerX;
        playerStartOnLoadY = playerY;
    }
	public override void Init(Vector3 worldPoint)
    {
        base.Init(worldPoint);
    }

    public override void TryInteract(GameObject user)
    {
        base.TryInteract(user);

        Debug.Log("trying to interact!");
    }

    public override void Interact()
    {
        base.Interact();
        Debug.Log(interactor.name + " has interacted with " + gameObject.name);
        AreaController.instance.GenerateArea(areaIndexToLoad, playerStartOnLoadX, playerStartOnLoadY);
    }

}
