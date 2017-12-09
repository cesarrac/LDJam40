using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal_Interactable : Interactable {

	TerminalController terminal_controller;
	void Awake(){
		terminal_controller = GetComponent<TerminalController>();
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
        terminal_controller.OnInteractWithTerminal();
    }
}
