using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MissionUIController : MonoBehaviour {
	public static MissionUIController instance {get; protected set;}
	public Text missionName, missionQuota;
	public GameObject missionPanel, missionPopUp;
	Animator missionAnim;

	void Awake(){
		instance = this;
		missionAnim = missionPanel.GetComponent<Animator>();
	}

	public void OnMissionChanged(Mission currMission){
		if (currMission == null)
			return;
		missionPanel.SetActive(true);
		missionName.text = currMission.name;
		missionQuota.text = currMission.deliveryQuota.ToString();
		missionAnim.SetTrigger("onMissionChanged");
	}

	public void DoPopUp(){
		//missionPopUp.SetActive(false);
		OnMissionChanged(TerminalController.instance.curMission);
	}
}
