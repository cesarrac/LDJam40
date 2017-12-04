using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : State
{	
	AI_Controller aI_Controller;
	public EnemyIdleState(StateType sType, AI_Controller aiControl) : base(sType){
		aI_Controller = aiControl;
	}
	public override void Enter(){
		// Try to get a target
		aI_Controller.target = AreaController.instance.character_generator.hero_GObj;
		// If you got one, Seek!
		if (aI_Controller.target != null){
			// enter seek state
			aI_Controller.PushState(StateType.Seek);
			return;
		}

		// no target... 
		// Pool?
	}
    public override void Update(float deltaTime)
    {
    }
}
