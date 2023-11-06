using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCrouch : StateBase
{
    private Player player;
    private float superJumpCharge;
    private bool superJumpTriggered;

    public override void OnStateEnter(params object[] objs)
    {
        player = (Player)objs[0];
        player.GetComponent<Animator>().SetTrigger("Idle");
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        superJumpCharge = 0;
        superJumpTriggered = false;
    }

    public override void OnStateStay()
    {
        CheckStateSwitch();
        player.Crouch();

        if(Input.GetKey(KeyCode.Space))
        {
            superJumpCharge += Time.deltaTime;
        }
        else
        {
            if(superJumpCharge > 0.15f)
            {
                player.SuperJump(superJumpCharge);
                player.usingSuperJump = true;
                superJumpTriggered = true;
            }
            superJumpCharge = 0;
        }


    }

    public override void CheckStateSwitch()
    {
        if (Input.GetKeyUp(KeyCode.S) && !superJumpTriggered)
        {
            player.stateMachine.SwitchState(Player.States.IDLE, player);
        }else if (superJumpTriggered && !player.usingSuperJump)
        {
            player.stateMachine.SwitchState(Player.States.IDLE, player);
        }

    }
}
