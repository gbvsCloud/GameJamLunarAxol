

public class EnemyState
{
    EnemyStateMachine stateMachine;
    Enemy enemy;

    public EnemyState(EnemyStateMachine stateMachine, Enemy enemy)
    {
        this.stateMachine = stateMachine;
        this.enemy = enemy;
    }

    public virtual void OnStateEnter()
    {

    }
    public virtual void OnStateStay()
    {

    }
    public virtual void OnStateExit()
    {

    }

}
