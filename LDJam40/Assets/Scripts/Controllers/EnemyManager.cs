using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager:MonoBehaviour{
    public static EnemyManager instance {get; protected set;}
    List<GameObject>enemiesMap;
    ObjectPool pool;
    private void OnEnable(){
        instance = this;
        enemiesMap = new List<GameObject>();
    }
    void GenerateEnemyNests(){

    }
    public void SpawnEnemies(int spawnYield, Vector2 nest_location){
        if (pool == null)
            pool = ObjectPool.instance;

        int[] statValues = new int[]{2, 1, 0};
        int startingHP = 6;

        for(int i = 0; i < spawnYield; i++){
            GameObject enemyGobj = pool.GetObjectForType("Enemy", true, nest_location);
            enemyGobj.GetComponent<CombatController>().Init(statValues,startingHP);
            enemyGobj.GetComponent<LerpMove_Controller>().Init(statValues[1]);
            enemyGobj.GetComponent<AI_Controller>().Init();
            enemyGobj.GetComponent<HealthController>().onHPZero += PoolEnemy;

            enemiesMap.Add(enemyGobj);
        }
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
    }
}

public struct EnemyNest{

    public int disturbance_level;
    public int totalEnemies;
    public int totalSpawned;
    public float spawnRate;
    public int spawnYield;
    public Vector2 nest_location;

}