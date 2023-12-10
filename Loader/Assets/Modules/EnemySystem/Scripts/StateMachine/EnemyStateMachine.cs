using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    public Enemy enemy { get; }

    public EnemyStateReusableData reusable_data { get; }
    public EnemyIdleState idle_state;
    public EnemyPatrolState patrol_state;
    public EnemyRunState run_state;
    public EnemyAttackState attack_state;
    public EnemyStateMachine(Enemy enemy)
    {
        this.enemy = enemy;

        reusable_data = new EnemyStateReusableData();

        idle_state = new EnemyIdleState(this);

        patrol_state = new EnemyPatrolState(this);

        run_state = new EnemyRunState(this);

        attack_state = new EnemyAttackState(this);
    }
}
