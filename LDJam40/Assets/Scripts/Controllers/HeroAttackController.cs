using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttackController : MonoBehaviour{

    public GameObject shootingOrigin;
    Animator anim;
    float fireRate = 0.12f, curTimer;
    KeyInputController keyInputController;

    Vector2 shootDirection;
    HeroController heroController;
    bool isAttacking = false;
    ObjectPool pool;
    CombatController combat_controller;
    void Awake(){
        heroController = GetComponent<HeroController>();
        combat_controller = GetComponent<CombatController>();
    }
    public void Init(){
        anim = GetComponentInChildren<Animator>();
        pool = ObjectPool.instance;
        keyInputController = KeyInputController.instance;
        keyInputController.onShootPressed += StartAttack;
        keyInputController.onShootHeld += Shoot;
        keyInputController.onShootUp += StopAttack;
    }
    void StartAttack(){
        anim.SetTrigger("hero_Attack");
        isAttacking = true;
    }

    void Shoot(){
        
        if (curTimer >= fireRate){
               Debug.Log("playr shooting");

               shootDirection = new Vector2(anim.GetFloat("x"), anim.GetFloat("y"));
               shootingOrigin.transform.localPosition = new Vector2(shootDirection.x + 0.5f, shootDirection.y + 0.5f);
               
            // Spwn projectile and give it direction
            GameObject projectile = ObjectPool.instance.GetObjectForType("Projectile", true, shootingOrigin.transform.position);
            projectile.GetComponent<ProjectileController>().Init(shootDirection, combat_controller);
            curTimer = 0;
        }
        else{
            curTimer += Time.deltaTime;
        }
    }

    void StopAttack(){
        if (isAttacking){
            isAttacking = false;
            
            anim.SetTrigger("hero_Idle");
        }
    }
    void OnDisable(){
        if (keyInputController != null){
            keyInputController.onShootPressed -= StartAttack;
            keyInputController.onShootHeld -= Shoot;
            keyInputController.onShootUp -= StopAttack;
        }
    }
}