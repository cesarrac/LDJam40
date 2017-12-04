using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Controller : MonoBehaviour {

	// Controls decisions
	 // - Be idle
	 // - Seek player
	 // - Attack player
	 // - Die
	 public State CurrentState {get; protected set;}
	 public StackFSM StateMachine {get; protected set;}
	 public int disturbed_level {get; protected set;} // how pissed are they, directly tied to radiation
	 public GameObject target;
	 State[] enemy_states;
	// public EnemyMoveController move_controller {get; protected set;}
	public LerpMove_Controller move_controller {get; protected set;}
	 public CircleCollider2D interact_radius {get; protected set;}
	 public Animator anim {get;protected set;}
	
	 public CombatController combat_controller {get; protected set;}
	 void Awake(){
		 enemy_states = new State[]{
			 new EnemyIdleState(StateType.Idle, this),
			 new EnemyAttackState(StateType.Attack, this),
			 new EnemySeekState(StateType.Seek, this)
		 };
		
		 move_controller = GetComponent<LerpMove_Controller>();
		 interact_radius = GetComponentInChildren<CircleCollider2D>();
		 anim = GetComponentInChildren<Animator>();
		combat_controller = GetComponent<CombatController>();
	 }

	 public void Init(){
		StateMachine = new StackFSM();
		 PushState(StateType.Idle);
	 }

	 public void PushState(StateType stateType){
		 foreach(State state in enemy_states){
			 if (state.stateType == stateType){
				 StateMachine.Push(state);
			 }
		 }
	 }

	 private void Update(){

		CurrentState = StateMachine.GetCurrentState();
        if (CurrentState != null)
        {
            CurrentState.Update(Time.deltaTime);
        }
	 }
	 public void SetDestinationToTarget(){
		 if (target == null)
		 	return;
		 move_controller.SetDesiredPos(target.transform.position);
	 }
	 public bool IsInRange(){
		 if (target == null){
			 return false;
		 }
		 return interact_radius.OverlapPoint(target.transform.position);
	 }


}
