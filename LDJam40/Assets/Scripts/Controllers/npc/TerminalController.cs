using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalController : MonoBehaviour {

	// knows current mission, can give missions, and can begin dialogue with the player
	public static TerminalController instance {get; protected set;}
	AreaController area_controller;

	int oreStored = 0;
	int oreDelivered = 0;

	public List<Mission> CompletedMissions {get; protected set;}
	public List<Mission> AvailableMissions {get; protected set;}
	public Mission curMission {get; protected set;}
	public Terminal_Interactable terminal_Interactable {get; protected set;}
	MissionManager mission_manager;
	public Dialogue[] missionDialogues;
	MissionUIController missionUI;
	int curMissionIndex = 0;
	private void Awake(){
		instance = this;
		terminal_Interactable = GetComponent<Terminal_Interactable>();
		mission_manager = new MissionManager();
		AvailableMissions = mission_manager.GetPrototypeMissions();
		CompletedMissions = new List<Mission>(AvailableMissions.Count);

	}
	public void Init(){
		area_controller = AreaController.instance;
		missionUI = MissionUIController.instance;
		if (curMission == null){
			// Get a mission
			GetNewMission();
		} 
	}

	void TriggerDialogue(Dialogue dialogue){
		DialogueManager.instance.StartDialogue(dialogue);
	}
	public void OnInteractWithTerminal(){
		SoundController.Instance.PlaySound("approve");
		if (CheckMissionComplete() == true){
			// do mission complete dialogue
			// and get new mission
			RadiationTracker.instance.GetClean();
			OnMissionComplete();
		}else{
			//repeat mission quota dialogue 
			TriggerDialogue(missionDialogues[curMissionIndex]);
			Debug.Log(curMission.name + " still active");
		}
	}
	public bool CheckMissionComplete(){
		int totalOreDelivered = Inventory.instance.GetAllOf(ExtractableType.Radiant);
		if (totalOreDelivered <= 0)
			return false;
		oreStored += totalOreDelivered;
		if (oreStored >= curMission.deliveryQuota){
			return true;
		}
		OnOreReceived();
		return false;
	}

	void OnMissionComplete(){
		if (curMission == null)
			return;
		Debug.Log(curMission.name + " completed");
		AvailableMissions.Remove(curMission);
		CompletedMissions.Add(curMission);
		if (AvailableMissions.Count > 0){
			curMissionIndex++;
			TriggerDialogue(missionDialogues[curMissionIndex]);
		}
		GetNewMission();
	}

	void OnOreReceived(){
		// Tell the player the ore was received and 
		// how much is now stored -- NEED DIALOGUE SYSTEM!
	}

	public void GetNewMission(){
		
		if (AvailableMissions.Count <= 0){
			// GAME COMPLETE!!!
		}else{
			// grab a random mission
			curMission = AvailableMissions[0];
			Debug.Log("Mission: " + curMission.name + " for " + curMission.deliveryQuota + " started");
			missionUI.OnMissionChanged(curMission);
		}
	}
	public void Pool(){
		ObjectPool.instance.PoolObject(this.gameObject);
	}
}

public class Mission{
	public string name = "New Mission";
	public int deliveryQuota {get; protected set;}

	public bool isComplete {get; protected set;}

	public Mission(string missionName, int quota){
		name = missionName;
		deliveryQuota = quota;
	}

}
