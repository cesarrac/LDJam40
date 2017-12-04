using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttackController : MonoBehaviour{

    Animator anim;
    float fireRate, curTimer;
    KeyInputController keyInputController;
    public void Init(){
        anim = GetComponentInChildren<Animator>();
        keyInputController = KeyInputController.instance;
        keyInputController.onShootPressed += StartAttack;
        keyInputController.onShootHeld += Shoot;
        keyInputController.onShootUp += StopAttack;
    }
    void StartAttack(){
        anim.SetTrigger("hero_Attack");
    }

    void Shoot(){
     
        if (curTimer >= fireRate){
               Debug.Log("playr shooting");
        }
        else{
            curTimer += Time.deltaTime;
        }
    }

    void StopAttack(){

    }
    void OnDisable(){
        if (keyInputController != null){
            keyInputController.onShootPressed -= StartAttack;
            keyInputController.onShootHeld -= Shoot;
            keyInputController.onShootUp -= StopAttack;
        }
    }
}