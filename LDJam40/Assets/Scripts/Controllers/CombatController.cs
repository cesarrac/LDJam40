using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CombatController:MonoBehaviour{

    Stat[] stats;
    HealthController health_controller;
    public void Init(int[] statValues, float startingHitPoints, bool isplayer){
        health_controller = GetComponent<HealthController>();
        health_controller.Init(startingHitPoints, isplayer);
        if (statValues.Length <= 3){
            stats = new Stat[3];
            stats[0] = new Stat(statValues[0], StatType.Damage);
            stats[1] = new Stat(statValues[1], StatType.MoveSpeed);
            stats[2] = new Stat(statValues[2], StatType.Armor);
        }
        else if (statValues.Length == 5){
            stats = new Stat[5];
            stats[0] = new Stat(statValues[0], StatType.Damage);
            stats[1] = new Stat(statValues[1], StatType.MoveSpeed);
            stats[2] = new Stat(statValues[2], StatType.Armor);
            stats[3] = new Stat(statValues[3], StatType.YieldModifier);
            stats[4] = new Stat(statValues[3], StatType.YieldRateModifier);
        }
    }
    public void ReceiveDamage(float dmg){
         health_controller.ReceiveDamage(dmg - stats[2].GetValue());
         Debug.Log(gameObject.name + " receives " + (dmg - stats[2].GetValue()) + " dmg!");
    }
    public void DoDamage(CombatController target){
        if (target == null)
            return;
        Debug.Log(this.gameObject.name + " does " + stats[0].GetValue() + " dmg!");
        target.ReceiveDamage(stats[0].GetValue());
    }
    
    public void ModifyStat(StatType sType, float modifier){
        foreach (Stat stat in stats){
            if (stat.statType == sType){
                stat.AddModifier(modifier);
                break;
            }
        }
    }
    public void ModifyAllStats(float modifier){
        foreach (Stat stat in stats){
            stat.AddModifier(modifier);
        }
    }
    public Stat GetStat(StatType sType){
        foreach (Stat stat in stats){
            if (stat.statType == sType){
                return stat;
            }
        }
        return null;
    }
    public float GetHitpoints(){
        return health_controller.HitPoints;
    }
}