using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenerator  {

    string heroName = "Hero";
    public GameObject hero_GObj {get;protected set;}
    ObjectPool pool;

    Inventory active_inventory;
    public CharacterGenerator(string name = "Hero"){
        heroName = name;
        pool = ObjectPool.instance;
        active_inventory = new Inventory(100);
    }
    public void SpawnCharacter(int startX = 0, int startY = 0){
        int[] statValues = new int[]{1, 3, 0, 0, 0};
        int startingHP = 6;

        hero_GObj = pool.GetObjectForType(heroName, true, new Vector2(startX, startY));
        hero_GObj.GetComponent<HeroController>().Init(active_inventory, statValues[1]);
        hero_GObj.GetComponent<CombatController>().Init(statValues, startingHP, true);
        hero_GObj.GetComponent<HealthController>().onDeath += OnCharacterDeath;
    }

    public void OnCharacterDeath(){
        // Show a screen // do animations // pool game obj
        // show menu to restart
        // hero_GObj.GetComponent<HealthController>().onDeath -= OnCharacterDeath;
        InventoryUI.instance.FadeInRestartMenu();
        SoundController.Instance.MuteAmbient();
         PoolCharacter();
    }

    public void PoolCharacter(){
        // update the active_inventory so you don't lose it
        // NOTE: The inventory should NOT be saved if the player died
        active_inventory = hero_GObj.GetComponent<HeroController>().heroInventory;
        pool.PoolObject(hero_GObj);
       // hero_GObj = null;
       
    }
}