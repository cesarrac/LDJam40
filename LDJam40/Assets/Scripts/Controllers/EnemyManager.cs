using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager:MonoBehaviour{
    public static EnemyManager instance {get; protected set;}
    List<GameObject>enemiesMap;
    ObjectPool pool;
    public EnemyNest[] enemy_nests {get;protected set;}
    private void OnEnable(){
        instance = this;
        enemiesMap = new List<GameObject>();
    }
    public void GenerateEnemyNests(Area activeArea){
        if (enemy_nests != null){
            return; // Don't re-do nests, keep old ones
        }

        enemy_nests = new EnemyNest[]{

            new EnemyNest("Nest_0",new Vector2(2, activeArea.Height - 2)),
            new EnemyNest("Nest_1",new Vector2(activeArea.Width - 2, activeArea.Height - 2)),
            new EnemyNest("Nest_2",new Vector2(activeArea.Width - 2, 2)),
            new EnemyNest("Nest_3",new Vector2(2, 2))
        };
    }
    public void SpawnEnemies(int nestIndex){
        if (pool == null)
            pool = ObjectPool.instance;
        if (AreaController.instance.active_area.areaType == AreaType.Interior){
            return;
        }
        int spawnYield;
        Vector2 nest_location;
        int[] statValues = new int[]{2, 2, 0};
        float startingHP = 2;

        spawnYield = enemy_nests[nestIndex].GetNextSpawnYield();
        nest_location = enemy_nests[nestIndex].nest_location;
        for(int i = 0; i < spawnYield; i++){
            GameObject enemyGobj = pool.GetObjectForType("Enemy", true, nest_location);
            
            CombatController combatControl = enemyGobj.GetComponent<CombatController>();
            float hpModifier = enemy_nests[nestIndex].GetDisturbanceHitPointsMod();
            if (hpModifier > 0){
                startingHP = hpModifier;
            }
            combatControl.Init(statValues,startingHP, false);

            float statModifier = enemy_nests[nestIndex].GetDisturbanceMod();
            if (statModifier > 0){
                combatControl.ModifyAllStats(statModifier);
            }
            
        // enemyGobj.GetComponent<LerpMove_Controller>().Init(statValues[1]);
            enemyGobj.GetComponent<EnemyMoveController>().Init(statValues[1]);
            enemyGobj.GetComponent<AI_Controller>().Init();
            enemyGobj.GetComponent<HealthController>().onHPZero += PoolEnemy;

            enemiesMap.Add(enemyGobj);
        }

       enemy_nests[nestIndex].SetTotalSpawned(enemiesMap.Count);
    }
    public void PoolEnemies(){
        if (enemiesMap.Count <= 0)
            return;
        foreach(GameObject gobj in enemiesMap){
            pool.PoolObject(gobj);
        }
        enemiesMap.Clear();
    }
    public void PoolEnemy(GameObject enemy){
        enemy.GetComponent<HealthController>().onHPZero -= PoolEnemy;
        pool.PoolObject(enemy);
        enemiesMap.Remove(enemy);
    }

    public void UpdateNestsDisturbance(int newRadAmmnt){
        for(int i = 0; i < enemy_nests.Length; i++){
            enemy_nests[i].UpdateNestDisturbance(newRadAmmnt);
            if (enemy_nests[i].totalEnemies > 0){
                // Should we spawn?
                SpawnEnemies(i);
            }
        }
    }
}

public struct EnemyNest{
    string name;
    public int disturbance_level;
    public int totalEnemies;
    public int totalSpawned;
    public float spawnRate;
    public int spawnYield;
    public Vector2 nest_location;

    public EnemyNest(string _name, Vector2 location){
        disturbance_level = 0;
        totalEnemies = 0; // enemies total belonging to nest (including spwnd)
        totalSpawned = 0; // total already spawned
        spawnRate = 1;      // how many spawn at a time
        spawnYield = 0;
        nest_location = location;
        name = _name;
    }

    public void UpdateNestDisturbance(int radiationAmmnt){
        disturbance_level += radiationAmmnt;
        disturbance_level = Mathf.Clamp(disturbance_level, 0, 100);
        totalEnemies = disturbance_level / 10;
        totalEnemies = Mathf.Clamp(totalEnemies, 0, 20);
        Debug.Log(name + " updated to lvl: " + disturbance_level + " total enemies @ " + totalEnemies);
    }

    public int GetNextSpawnYield(){ // Spawn a quarter of total enemies left not spwnd
        return spawnYield = Mathf.FloorToInt(totalEnemies - totalSpawned * 0.25f);
    }

    public float GetDisturbanceHitPointsMod(){
        return disturbance_level / 10;
    }
    public float GetDisturbanceMod(){
        return disturbance_level / 100;
    }
    public void SetTotalSpawned(int spawnCount){
        totalSpawned = spawnCount;
    }
}