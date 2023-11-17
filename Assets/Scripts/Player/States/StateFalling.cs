
using UnityEngine;

public class StateFalling : StateBase
{
    private Player player;
    public override void OnStateEnter(params object[] objs)
    {
        player = (Player)objs[0];
    }  
    public override void OnStateStay()
    {
        CheckStateSwitch();
    }

    public override void FixedUpdate()
    {
        player.Run();
    }


    public override void CheckStateSwitch()
    {
        if (player.isGrounded)
        {
            player.stateMachine.SwitchState(Player.States.IDLE, player);
        }
    }

    public override void OnStateExit()
    {
        player.isGrounded = false;
    }
}