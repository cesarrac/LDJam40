using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenerator  {

    string heroName = "Hero";
    public GameObject hero_GObj {get;protected set;}
    ObjectPool pool;
    public CharacterGenerator(string name = "Hero"){
        heroName = name;
        pool = ObjectPool.instance;
    }
    public void SpawnCharacter(int startX = 0, int startY = 0){
        hero_GObj = pool.GetObjectForType(heroName, true, new Vector2(startX, startY));
        hero_GObj.GetComponent<HeroController>().Init();
    }

    public void PoolCharacter(){
        pool.PoolObject(hero_GObj);
    }
}