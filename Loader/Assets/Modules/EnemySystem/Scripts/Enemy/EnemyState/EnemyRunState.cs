using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRunState : EnemyStateBase
{   
    public bool find_attack_target = false;
    public EnemyRunState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        find_attack_target = true;

        enemy_state_machine.enemy.agent.enabled = true;

        enemy_state_machine.enemy.agent.stoppingDistance = 1;

        enemy_state_machine.enemy.agent.speed = 4f;

        enemy_state_machine.enemy.animator.CrossFade("Run", 0.1f);
    }

    public override void OnTriggerStay(Collider collider)
    {
        // base.OnTriggerStay(collider);
        if(collider.tag == "Player" && find_attack_target)
        {
            enemy_state_machine.reusable_data.agent_target_pos = collider.transform.position;

            enemy_state_machine.enemy.agent.SetDestination(enemy_state_machine.reusable_data.agent_target_pos);
        }

    }
    public override void OnTriggerExit(Collider collider)
    {
        // base.OnTriggerStay(collider);
        if(collider.tag == "Player")
        {   
            enemy_state_machine.enemy.agent.SetDestination(enemy_state_machine.reusable_data.agent_target_pos);
            find_attack_target = false;
        }

    }
    public override void OnUpdate()
    {
        base.OnUpdate();

        ReachTargetPosAttack();
    }

    public void ReachTargetPosAttack()
    {
        if(!enemy_state_machine.enemy.agent.pathPending && enemy_state_machine.enemy.agent.remainingDistance > 2f) return;

        if(find_attack_target)
        {
            enemy_state_machine.ChangeState(enemy_state_machine.attack_state);
        }
        else
        {
            enemy_state_machine.ChangeState(enemy_state_machine.idle_state);
        }
    }
}
