using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : State
{	
	AI_Controller aI_Controller;
	float attackRate = 0.2f;
	float curTime;
	CombatController target_combatControl;
	public EnemyAttackState(StateType sType, AI_Controller aiControl) : base(sType){
		aI_Controller = aiControl;
	}
	public override void Enter(){
		curTime = 0;
		// Try to get a target
		aI_Controller.target = AreaController.instance.character_generator.hero_GObj;

		if (aI_Controller.target == null){
			// no target... 
			Finished();
			return;
		}
		target_combatControl = aI_Controller.target.GetComponent<CombatController>();
		
		aI_Controller.anim.ResetTrigger("idle");
		aI_Controller.anim.ResetTrigger("moving");
		aI_Controller.anim.SetTrigger("attacking");
	}


    public override void Update(float deltaTime)
    {
        if (curTime >= attackRate){
			
			Hit();
		}else{
			curTime += deltaTime;
		}
    }

	void Hit(){
		if (aI_Controller.IsInRange()){	
			aI_Controller.combat_controller.DoDamage(target_combatControl);
			curTime = 0;
		}
		Finished();
	}
	public override void Finished(){
		base.Finished();
		aI_Controller.anim.SetTrigger("idle");
		aI_Controller.StateMachine.Pop();
	}
}
