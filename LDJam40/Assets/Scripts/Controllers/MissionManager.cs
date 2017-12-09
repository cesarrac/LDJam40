using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager  {

	List<Mission> missionPrototypes;

	public MissionManager(){
		InitPrototypes();
	}
	void InitPrototypes(){
		missionPrototypes = new List<Mission>(){
			new Mission("Welcome", 5), 
			new Mission("Work hard", 10),
			new Mission("Work Harder", 20),
			new Mission("Do you have what it takes?", 50),
			new Mission("Ticket to citizenship", 80)
		};
	}

	public List<Mission> GetPrototypeMissions(){
		return missionPrototypes;
	}
}
