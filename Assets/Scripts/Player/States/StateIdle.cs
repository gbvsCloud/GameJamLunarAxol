using Unity.VisualScripting;
using UnityEngine;

public class StateIdle : StateBase
{ 
    private Player player;
    public override void OnStateEnter(params object[] objs)
    {
        player = (Player)objs[0];   
        player.GetComponent<Animator>().SetTrigger("Idle");
    }

    public override void OnStateStay()
    {
        CheckStateSwitch();
    }

    public override void CheckStateSwitch()
    {
        if(player.xInput != 0 && !player.usingSuperJump)
        {
            player.stateMachine.SwitchState(Player.States.RUNNING, player);
        }else if (Input.GetKeyDown(KeyCode.Space) && player.isGrounded)
        {
            player.stateMachine.SwitchState(Player.States.JUMPING, player);
        }else if(Input.GetKeyDown(KeyCode.S) && player.isGrounded)
        {
            player.stateMachine.SwitchState(Player.States.CROUCH, player);
        }
    }

}