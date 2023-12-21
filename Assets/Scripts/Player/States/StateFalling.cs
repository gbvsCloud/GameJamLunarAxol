
using UnityEngine;

public class StateFalling : StateBase
{
    private Player player;
    private Animator animator;
    public override void OnStateEnter(params object[] objs)
    {
        player = objs[0] as Player;
        animator = player?.GetComponent<Animator>();
        animator.SetTrigger("Fall");
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
        animator.ResetTrigger("Fall");
    }
}