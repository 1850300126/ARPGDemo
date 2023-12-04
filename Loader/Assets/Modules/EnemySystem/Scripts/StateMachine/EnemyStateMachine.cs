using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    public Enemy enemy { get; }

    public EnemyIdleState idle_state;
    public EnemyPatrolState patrol_state;
    public EnemyStateMachine(Enemy enemy)
    {
        this.enemy = enemy;

        idle_state = new EnemyIdleState(this);

        patrol_state = new EnemyPatrolState(this);
    }
}
