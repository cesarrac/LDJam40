using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySeekState : State {
	float seekRate = 1.5f;
	float curTimer;
	AI_Controller aI_Controller;
	public EnemySeekState(StateType sType, AI_Controller aiControl) : base(sType){
		aI_Controller = aiControl;
	}
	public override void Enter(){
		curTimer = 0;
		// Try to get a target
		aI_Controller.target = AreaController.instance.character_generator.hero_GObj;
		// If you got one, Seek!
		if (aI_Controller.target == null){
			// no target... 
			Finished();
		}

		
	}
    public override void Update(float deltaTime)
    {
		if (curTimer >= seekRate){
			CheckTarget();
		}else{
			curTimer += deltaTime;
		}
		
    }

	void CheckTarget(){
		if (aI_Controller.IsInRange() == false){
			aI_Controller.SetDestinationToTarget();
			curTimer = 0;
		}else{
			Attack();
		}
	}
	void Attack(){
		aI_Controller.PushState(StateType.Attack);
	}
	public override void Finished(){
		base.Finished();
		aI_Controller.anim.SetTrigger("idle");
		aI_Controller.StateMachine.Pop();
	}
	
}
