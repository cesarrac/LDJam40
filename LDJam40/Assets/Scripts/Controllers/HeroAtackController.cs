using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttackController : MonoBehaviour{

    Animator anim;

    void StartAttack(){
        anim.ResetTrigger("hero_Idle");
        anim.SetTrigger("hero_Attack");
    }
}