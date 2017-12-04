using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RadiationLevel{
	clean, negligible, minimal, dangerous, critical, lethal
}
public class RadiationTracker  {

	public static RadiationTracker instance {get; protected set;}
	int[] maxRadLevelValues = new int[System.Enum.GetValues(typeof(RadiationLevel)).Length];

	public RadiationLevel radLevel {get;protected set;}
	int pRad;
	int radiation {
		get{return pRad;}
		set{
			pRad = Mathf.Clamp(value, 0, 1000);
		}
	}
	public int Radiation {get{return radiation;}}

	public delegate void OnRadChange();
	public event OnRadChange onRadChange;
	public RadiationTracker(Inventory heroInventory){
		instance = this;
		int baseRadLevel = 20;
		// init max level values
		for(int i = 0; i < maxRadLevelValues.Length; i++){
			maxRadLevelValues[i] = baseRadLevel * i;
		}
		heroInventory.onInventoryChanged += OnRadiationReceived;
	}
	public void OnRadiationReceived(int oreInInventory){
		// THE  MORE YOU HAVE THE WORSE IT GETS!!!!!!
		int baseRad = 2;
		// calculate radiation to add to the current radiation by 
		// how many ores are now in the inventory!!
		int radAmmnt = oreInInventory * baseRad;
		radiation = radAmmnt;
		UpdateRadLevel(radiation);
	}
	void UpdateRadLevel(int rad){
		if (rad > 0 && rad <= maxRadLevelValues[1]){
			radLevel = RadiationLevel.negligible;
		}
		else if (rad > maxRadLevelValues[1] && rad <= maxRadLevelValues[2]){
			radLevel = RadiationLevel.minimal;
		}
		else if (rad > maxRadLevelValues[2] && rad <= maxRadLevelValues[3]){
			radLevel = RadiationLevel.dangerous;
		}
		else if (rad > maxRadLevelValues[3] && rad <= maxRadLevelValues[4]){
			radLevel = RadiationLevel.critical;
		}
		else {
			radLevel = RadiationLevel.lethal;
		}

		if (onRadChange != null)
			onRadChange();
		//Debug.Log("radiation level at: " + radLevel + "(value = " + radiation + ")");
	}
}
